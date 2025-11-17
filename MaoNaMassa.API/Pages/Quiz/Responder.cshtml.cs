using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MaoNaMassa.API.ViewModels;
using MaoNaMassa.Application.UseCases;
using MaoNaMassa.Application.DTOs.Input;
using MaoNaMassa.Domain.Interfaces;

namespace MaoNaMassa.API.Pages.Quiz;

public class ResponderModel : PageModel
{
    private readonly ResponderQuizUseCase _responderQuizUseCase;
    private readonly IQuizRepository _quizRepository;

    public ResponderModel(
        ResponderQuizUseCase responderQuizUseCase,
        IQuizRepository quizRepository)
    {
        _responderQuizUseCase = responderQuizUseCase;
        _quizRepository = quizRepository;
    }

    [BindProperty]
    public ResponderQuizViewModel ViewModel { get; set; } = new();

    public string? MensagemErro { get; set; }
    public string? MensagemSucesso { get; set; }

    public async Task OnGetAsync(Guid? quizId = null, Guid? usuarioId = null)
    {
        if (quizId.HasValue)
        {
            ViewModel.QuizId = quizId.Value;
            var quiz = await _quizRepository.GetByIdAsync(quizId.Value);
            if (quiz != null)
            {
                ViewModel.Pergunta = quiz.Pergunta;
                ViewModel.CursoTitulo = "Curso";
            }
        }

        if (usuarioId.HasValue)
        {
            ViewModel.UsuarioId = usuarioId.Value;
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            // Recarregar dados do quiz se necessário
            if (ViewModel.QuizId != Guid.Empty)
            {
                var quiz = await _quizRepository.GetByIdAsync(ViewModel.QuizId);
                if (quiz != null)
                {
                    ViewModel.Pergunta = quiz.Pergunta;
                    ViewModel.CursoTitulo = "Curso";
                }
            }
            return Page();
        }

        try
        {
            var dto = new MaoNaMassa.Application.DTOs.ResponderQuizDTO
            {
                QuizId = ViewModel.QuizId,
                Resposta = ViewModel.Resposta
            };

            var resultado = await _responderQuizUseCase.ExecuteAsync(ViewModel.UsuarioId, dto);

            if (resultado.Correta)
            {
                MensagemSucesso = $"Resposta correta! Parabéns!";
            }
            else
            {
                MensagemSucesso = $"Resposta incorreta. A resposta correta é: {resultado.RespostaCorreta}";
            }

            // Limpar formulário
            ViewModel.Resposta = string.Empty;
            
            return Page();
        }
        catch (Exception ex)
        {
            MensagemErro = ex.Message;
            return Page();
        }
    }
}

