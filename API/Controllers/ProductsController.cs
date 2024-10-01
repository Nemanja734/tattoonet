using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProductsController(IUnitOfWork unit) : BaseApiController
{
    // Get Products from Db (with specification)
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery]ProductSpecParams specParams)
    {
        // Create object of class ProductSpecification
        var spec = new ProductSpecification(specParams);

        // Return list of products by accessing the BaseApiControllers.cs file
        return await CreatePagedResult(unit.Repository<Product>(),spec, specParams.PageIndex, specParams.PageSize);
    }

    // Get Products from Db with Id
    [HttpGet("{id:int}")] // api/products/2
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await unit.Repository<Product>().GetByIdAsync(id);

        if (product == null) return NotFound();

        return product;
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        var spec = new BrandListSpecification();

        return Ok(await unit.Repository<Product>().ListAsync(spec));
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var spec = new TypeListSpecification();

        return Ok(await unit.Repository<Product>().ListAsync(spec));
    }

    // Create Product in Db
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        unit.Repository<Product>().Add(product);

        if (await unit.Complete())
        {
            return CreatedAtAction("GetPRoduct", new {id = product.Id}, product);
        }

        return BadRequest("Problem creating product");
    }

    // Update Product
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (product.Id != id || !ProductExists(id))
            return BadRequest("Cannot update this product");

        unit.Repository<Product>().Update(product);

        if (await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem updating the product");
    }

    // Delete Product
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await unit.Repository<Product>().GetByIdAsync(id);

        if (product == null) return NotFound();

        unit.Repository<Product>().Remove(product);

        if (await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting the product");
    }

    // Check if Product exists in Db
    private bool ProductExists(int id)
    {
        if (unit.Repository<Product>().Exists(id))
            return true;
        
        return false;
    }
}
