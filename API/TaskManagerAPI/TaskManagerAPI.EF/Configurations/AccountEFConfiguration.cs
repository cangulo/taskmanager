using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TaskManagerAPI.Models.BE;

namespace TaskManagerAPI.EF.Configurations
{
    /// <summary>
    /// Thanks to https://codeburst.io/ientitytypeconfiguration-t-in-entityframework-core-3fe7abc5ee7a
    /// </summary>
    public class AccountEFConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Accounts");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            builder.Property(x => x.Email).HasMaxLength(60).IsRequired();
            builder.Property(x => x.Username).HasMaxLength(60).IsRequired();
            builder.Property(x => x.Password).HasMaxLength(20).IsRequired();
            builder.Property(x => x.FailedLoginAttempts).HasDefaultValue(0).HasMaxLength(10).IsRequired();
            builder.Property(x => x.PhoneNumber).HasMaxLength(20).IsRequired();
            builder.Property(x => x.LastLogintime).HasDefaultValue(DateTime.MinValue).IsRequired();
            builder.Property(x => x.Token);
        }
    }
}