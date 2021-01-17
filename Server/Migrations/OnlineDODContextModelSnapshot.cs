﻿// <auto-generated />
using System;
using BP_OnlineDOD.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BP_OnlineDOD.Server.Migrations
{
    [DbContext(typeof(OnlineDODContext))]
    partial class OnlineDODContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BP_OnlineDOD.Shared.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ParentMessageId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ThumbsUpCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeSent")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ParentMessageId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("BP_OnlineDOD.Shared.Models.Message", b =>
                {
                    b.HasOne("BP_OnlineDOD.Shared.Models.Message", "ParentMessage")
                        .WithMany("ChildMessages")
                        .HasForeignKey("ParentMessageId");
                });
#pragma warning restore 612, 618
        }
    }
}
