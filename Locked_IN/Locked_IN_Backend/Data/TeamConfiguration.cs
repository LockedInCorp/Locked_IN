using Locked_IN_Backend.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Locked_IN_Backend.Data;

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.HasIndex(b => b.Name)
            .HasMethod("GIN")
            .IsTsVectorExpressionIndex("english");
    }
}