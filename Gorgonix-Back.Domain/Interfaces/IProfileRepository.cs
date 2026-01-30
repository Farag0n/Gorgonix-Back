using Gorgonix_Back.Domain.Entities;

namespace Gorgonix_Back.Domain.Interfaces;

public interface IProfileRepository
{
    Task<IEnumerable<Profile>> GetProfilesByUserIdAsync(Guid userId);
    Task<Profile?> GetProfileByIdAsync(Guid profileId);
    
    // --- LÓGICA DE FAVORITOS ---
    // Devuelve directamente los OBJETOS Content que están en favoritos de ese perfil
    Task<IEnumerable<Content>> GetFavoriteContentsAsync(Guid profileId);
    
    // Verificar si ya es favorito (para el botón de like/dislike)
    Task<bool> IsContentFavoriteAsync(Guid profileId, Guid contentId);
    
    // Agregar/Remover favorito (manipulando la tabla intermedia)
    Task AddFavoriteAsync(Favorite favorite);
    Task RemoveFavoriteAsync(Guid profileId, Guid contentId);
    // ---------------------------

    Task AddAsync(Profile profile);
    Task UpdateAsync(Profile profile);
    Task DeleteAsync(Profile profile);
}