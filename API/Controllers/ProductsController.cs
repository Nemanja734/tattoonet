using System;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IGenericRepository<Product> repo) : ControllerBase
{
    // Get Products from Db
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
    {
        return Ok(await repo.ListAllAsync());
    }

    // Get Products from Db with Id
    [HttpGet("{id:int}")] // api/products/2
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await repo.GetByIdAsync(id);

        if (product == null) return NotFound();

        return product;
    }

    // TODO: Implement method
    // [HttpGet("brands")]
    // public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    // {
    //     return Ok(await repo.GetBrandsAsync());
    // }

    // [HttpGet("types")]
    // public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    // {
    //     return Ok(await repo.GetTypesAsync());
    // }

    // Create Product in Db
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        repo.Add(product);

        if (await repo.SaveAllAsync())
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

        repo.Update(product);

        if (await repo.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem updating the product");
    }

    // Delete Product
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await repo.GetByIdAsync(id);

        if (product == null) return NotFound();

        repo.Remove(product);

        if (await repo.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting the product");
    }

    // Check if Product exists in Db
    private bool ProductExists(int id)
    {
        if (repo.Exists(id))
            return true;
        
        return false;
    }
}
