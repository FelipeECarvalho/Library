﻿using AutoMapper;
using Library.Application.DTOs;
using Library.Application.InputModels.Books;
using Library.Core.Entities;
using Library.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [Route("api/v1/books")]
    public class BooksController(IBookService _bookService, IMapper _mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = await _bookService.GetAllAsync();

            if (books is null || books.Count == 0)
                return NoContent();

            var dto = _mapper.Map<IList<BookDto>>(books);

            return Ok(dto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _bookService.GetByIdAsync(id);

            if (book is null) 
                return NotFound();

            var dto = _mapper.Map<BookDto>(book);

            return Ok(dto);
        }

        [HttpGet("{title}")]
        public async Task<IActionResult> GetByTitle(string title)
        {
            var books = await _bookService.GetByTitleAsync(title);

            if (books is null || books.Count == 0)
                return NoContent();

            var dto = _mapper.Map<IList<BookDto>>(books);

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BookCreateInputModel model)
        {

            var book = _mapper.Map<Book>(model);

            await _bookService.CreateAsync(book);
            return CreatedAtAction(nameof(GetById), new { id = book.Id}, book);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _bookService.GetByIdAsync(id);

            if (book is null)
                return NotFound();

            await _bookService.DeleteAsync(book);
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] BookUpdateInputModel model)
        {
            var book = await _bookService.GetByIdAsync(id);

            if (book is null)
                return NotFound();

            _mapper.Map(model, book);

            await _bookService.UpdateAsync(book);
            return NoContent();
        }

        [HttpPut("{id:int}/update-stock/{stockNumber:int}")]
        public async Task<IActionResult> Put(int id, int stockNumber)
        {
            if (stockNumber < 0)
                return BadRequest("Stock number cannot be negative.");

            var book = await _bookService.GetByIdAsync(id);

            if (book is null)
                return BadRequest();

            book.StockNumber = stockNumber;

            await _bookService.UpdateAsync(book);
            return NoContent();
        }
    }
}