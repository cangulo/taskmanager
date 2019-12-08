using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TaskManagerAPI.Models.BE;
using TaskManagerAPI.Models.BE.Tasks;

namespace TaskManagerAPI.EF.Configurations
{
    public class TaskEFConfiguration : IEntityTypeConfiguration<TaskDomain>
    {
        public void Configure(EntityTypeBuilder<TaskDomain> builder)
        {
            builder.ToTable("Tasks");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            builder.Property(x => x.Title).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.DateToBeFinished).HasDefaultValue(DateTime.MinValue);
            builder.Property(x => x.DateToBeNotified).HasDefaultValue(DateTime.MinValue);
            builder.HasOne<Account>(x => x.Account).
                WithMany(x => x.Tasks).
                HasForeignKey(x => x.AccountId).
                OnDelete(DeleteBehavior.Cascade);
        }
    }
}