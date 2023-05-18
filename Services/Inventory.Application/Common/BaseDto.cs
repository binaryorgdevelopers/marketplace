using Mapster;

namespace Marketplace.Application.Common;

public abstract class BaseDto<TDto, TEntity> : IRegister
    where TDto : class
    where TEntity : class
{
    private TypeAdapterConfig Config;


    public virtual void AddCustomMappings()
    {
    }

    public void Register(TypeAdapterConfig config)
    {
        Config = config;
    }

    public TEntity ToEntity()
    {
        return this.Adapt<TEntity>();
    }

    public TEntity ToEntity(TEntity entity)
    {
        return (this as TDto).Adapt(entity);
    }

    public static TDto FromEntity(TEntity entity)
    {
        return entity.Adapt<TDto>();
    }

    protected TypeAdapterSetter<TDto, TEntity> SetCustomMappings()
        => Config.ForType<TDto, TEntity>();

    protected TypeAdapterSetter<TEntity, TDto> SetCustomMappingsInverse()
        => Config.ForType<TEntity, TDto>();
}