﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TaskManagerAPI.Models.BE;

namespace TaskManagerAPI.EF.Configurations
{
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
            builder.Property(x => x.Token).HasDefaultValue(string.Empty);
            // https://medium.com/agilix/entity-framework-core-enums-ee0f8f4063f2
            builder.Property(x => x.Status).HasDefaultValue(UserStatus.Active).HasConversion<int>().IsRequired();
            // TODO: IMPROVEMENT - It will be better if we set status of pending verification for accounts created and wait until the user verify the account in an email link we send
        }
    }
}