using MaoNaMassa.Domain.Entities;

namespace MaoNaMassa.Domain.Interfaces;

public interface IServicoRepository
{
    Task<Servico?> GetByIdAsync(Guid id);
    Task<IEnumerable<Servico>> GetAllAsync();
    Task<IEnumerable<Servico>> GetByProfissionalIdAsync(Guid profissionalId);
    Task<IEnumerable<Servico>> GetByCidadeAsync(string cidade);
    Task<IEnumerable<Servico>> SearchAsync(string? cidade, string? termo);
    Task<Servico> CreateAsync(Servico servico);
    Task UpdateAsync(Servico servico);
    Task DeleteAsync(Guid id);
}

