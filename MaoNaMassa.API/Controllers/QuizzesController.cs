using MaoNaMassa.API.Helpers;
using MaoNaMassa.Application.DTOs.Input;
using MaoNaMassa.Application.DTOs.Output;
using MaoNaMassa.Domain.Entities;
using MaoNaMassa.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MaoNaMassa.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizzesController : ControllerBase
{
    private readonly IQuizRepository _repository;

    public QuizzesController(IQuizRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Busca todos os quizzes de um curso
    /// </summary>
    [HttpGet("curso/{cursoId}")]
    public async Task<ActionResult<IEnumerable<QuizResponse>>> GetByCursoId(Guid cursoId)
    {
        var quizzes = await _repository.GetByCursoIdAsync(cursoId);
        var responses = quizzes.Select(q => MapToResponse(q)).ToList();

        return Ok(new { data = responses });
    }

    /// <summary>
    /// Busca um quiz por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<QuizResponse>> GetById(Guid id)
    {
        var quiz = await _repository.GetByIdAsync(id);
        if (quiz == null)
        {
            return NotFound();
        }

        var response = MapToResponse(quiz);
        var links = HateoasHelper.CreateResourceLinks(
            $"{Request.Scheme}://{Request.Host}/api/quizzes",
            id,
            includeDelete: false);

        return Ok(new { data = response, links });
    }

    /// <summary>
    /// Cria um novo quiz
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<QuizResponse>> Create([FromBody] CriarQuizRequest request)
    {
        var quiz = new Quiz(
            request.CursoId,
            request.Pergunta,
            request.RespostaCorreta);

        var quizCriado = await _repository.CreateAsync(quiz);
        var response = MapToResponse(quizCriado);

        var links = HateoasHelper.CreateResourceLinks(
            $"{Request.Scheme}://{Request.Host}/api/quizzes",
            quizCriado.Id,
            includeDelete: false);

        return CreatedAtAction(
            nameof(GetById),
            new { id = quizCriado.Id },
            new { data = response, links });
    }

    /// <summary>
    /// Atualiza um quiz
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<QuizResponse>> Update(Guid id, [FromBody] CriarQuizRequest request)
    {
        var quiz = await _repository.GetByIdAsync(id);
        if (quiz == null)
        {
            return NotFound();
        }

        quiz.SetPergunta(request.Pergunta);
        quiz.SetRespostaCorreta(request.RespostaCorreta);

        await _repository.UpdateAsync(quiz);

        var response = MapToResponse(quiz);
        var links = HateoasHelper.CreateResourceLinks(
            $"{Request.Scheme}://{Request.Host}/api/quizzes",
            id,
            includeDelete: false);

        return Ok(new { data = response, links });
    }

    /// <summary>
    /// Remove um quiz
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var quiz = await _repository.GetByIdAsync(id);
        if (quiz == null)
        {
            return NotFound();
        }

        await _repository.DeleteAsync(id);
        return NoContent();
    }

    private QuizResponse MapToResponse(Quiz quiz)
    {
        return new QuizResponse
        {
            Id = quiz.Id,
            CursoId = quiz.CursoId,
            Pergunta = quiz.Pergunta
            // Não retornamos RespostaCorreta por segurança
        };
    }
}

