using Locked_IN_Backend.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Locked_IN_Backend.Data;

public class ChatConfiguration : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.HasIndex(b => b.Name)
            .HasMethod("GIN")
            .IsTsVectorExpressionIndex("english");
    }
}