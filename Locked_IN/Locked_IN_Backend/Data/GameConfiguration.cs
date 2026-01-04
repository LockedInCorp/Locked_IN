using Locked_IN_Backend.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Locked_IN_Backend.Data;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.HasIndex(b => b.Name)
            .HasMethod("GIN")
            .IsTsVectorExpressionIndex("english");
    }
}