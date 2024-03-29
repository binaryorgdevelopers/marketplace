﻿using Inventory.Domain.Entities;

namespace Marketplace.Application.Common.Messages.Messages;

public class CharacteristicsDto : BaseDto<CharacteristicsDto, Characteristics>
{
    public CharacteristicsDto(Guid id, string title, IEnumerable<ColorRead> values)
    {
        Id = id;
        Title = title;
        Values = values;
    }

    public Guid Id { get; set; }
    public string Title { get; set; }
    public IEnumerable<ColorRead> Values { get; set; }

    public CharacteristicsDto()
    {
        
    }
}