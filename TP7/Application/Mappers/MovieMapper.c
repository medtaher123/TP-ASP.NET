
public static class MovieMapper
{
    // Map from MovieEntity to MovieDto
    public static MovieDto ToDto(MovieEntity entity)
    {
        if (entity == null) return null;

        return new MovieDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Director = entity.Director,
            Genre = entity.Genre,
            ReleaseYear = entity.ReleaseYear
        };
    }

    // Map from MovieDto to MovieEntity
    public static MovieEntity ToEntity(MovieDto dto)
    {
        if (dto == null) return null;

        return new MovieEntity
        {
            Id = dto.Id,
            Title = dto.Title,
            Director = dto.Director,
            Genre = dto.Genre,
            ReleaseYear = dto.ReleaseYear
        };
    }

    // Map a collection of MovieEntity to a collection of MovieDto
    public static IEnumerable<MovieDto> ToDtoList(IEnumerable<MovieEntity> entities)
    {
        if (entities == null) return null;

        return entities.Select(entity => ToDto(entity)).ToList();
    }

    // Map a collection of MovieDto to a collection of MovieEntity
    public static IEnumerable<MovieEntity> ToEntityList(IEnumerable<MovieDto> dtos)
    {
        if (dtos == null) return null;

        return dtos.Select(dto => ToEntity(dto)).ToList();
    }
}
