using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Core.DTOs;
using ProductAPI.Core.Models;
using ProductAPI.Core.Services;
using ProductAPI.Service.Exceptions;

namespace ProductAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : CustomBasesController
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;

        public ProductsController(IMapper mapper, IProductService productService)
        {
            _mapper = mapper;
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var products = await _productService.GetAllAsync();
            var productsDto = _mapper.Map<List<ProductDto>>(products.ToList());
            return CreateActionResult(GlobalResultDto<List<ProductDto>>.Success(200, productsDto));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var product = await _productService.GetById(id);
            var productDto = _mapper.Map<ProductDto>(product);
            return CreateActionResult(GlobalResultDto<ProductDto>.Success(200, productDto));
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductDto productDto)
        {
            var product = await _productService.AddAsync(_mapper.Map<Product>(productDto));
            var productDtos = _mapper.Map<ProductDto>(product);
            return CreateActionResult(GlobalResultDto<ProductDto>.Success(201, productDtos));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update(ProductDto productDto)
        {
            await _productService.UpdateAsync(_mapper.Map<Product>(productDto));
            return CreateActionResult(GlobalResultDto<NoContentDto>.Success(204));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(string id)
        {
            var product = await _productService.GetById(id);
            await _productService.RemoveAsync(product);
            return CreateActionResult(GlobalResultDto<NoContentDto>.Success(204));
        }
    }
}
