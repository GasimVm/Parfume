﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Parfume.DAL;

namespace Parfume.Migrations
{
    [DbContext(typeof(ParfumeContext))]
    [Migration("20240116150917_ChangeIntToString")]
    partial class ChangeIntToString
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Parfume.Models.Bonus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double?>("Amount")
                        .HasColumnType("float");

                    b.Property<int?>("CardNumber")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<int>("Precent")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Bonus");
                });

            modelBuilder.Entity("Parfume.Models.BonusCard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double?>("Balans")
                        .HasColumnType("float");

                    b.Property<int>("BonusCardTypeId")
                        .HasColumnType("int");

                    b.Property<string>("CardNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.HasKey("Id");

                    b.HasIndex("BonusCardTypeId");

                    b.HasIndex("CustomerId");

                    b.ToTable("BonusCards");
                });

            modelBuilder.Entity("Parfume.Models.BonusCardHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double?>("Amount")
                        .HasColumnType("float");

                    b.Property<int?>("BonusCardId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BonusCardId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("OrderId");

                    b.ToTable("BonusCardHistories");
                });

            modelBuilder.Entity("Parfume.Models.BonusCardType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("BonusCardTypes");
                });

            modelBuilder.Entity("Parfume.Models.BonusHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double?>("Amount")
                        .HasColumnType("float");

                    b.Property<int?>("BonusId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<bool>("IsIncome")
                        .HasColumnType("bit");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BonusId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("OrderId");

                    b.ToTable("BonusHistories");
                });

            modelBuilder.Entity("Parfume.Models.Card", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<int>("Limit")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("Parfume.Models.CrediteHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("CachMany")
                        .HasPrecision(25, 4)
                        .HasColumnType("float(25)");

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int?>("PaymentHistoryId")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("PaymentHistoryId");

                    b.HasIndex("UserId");

                    b.ToTable("CrediteHistories");
                });

            modelBuilder.Entity("Parfume.Models.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BaseNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Birthday")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("BlockDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("BlockNote")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("BonusAmount")
                        .HasColumnType("float");

                    b.Property<int?>("BonusDegree")
                        .HasColumnType("int");

                    b.Property<int?>("CardId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("FatherName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fincode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstNumberWho")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InstagramAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<bool>("IsBlock")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool?>("IsVIP")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ReferencesCustomerId")
                        .HasColumnType("int");

                    b.Property<int?>("ReferencesId")
                        .HasColumnType("int");

                    b.Property<string>("SecondNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecondNumberWho")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ThirdNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ThirdNumberWho")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WhoIsOkey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WorkAddress")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CardId");

                    b.HasIndex("ReferencesCustomerId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Parfume.Models.Expense", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Money")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Expenses");
                });

            modelBuilder.Entity("Parfume.Models.Log", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BrowserInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Error")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fincode")
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RemoteIpAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Success")
                        .HasColumnType("bit");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("Parfume.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<double?>("BonusCardAmount")
                        .HasColumnType("float");

                    b.Property<int?>("BonusCardId")
                        .HasColumnType("int");

                    b.Property<double?>("BonusPrice")
                        .HasColumnType("float");

                    b.Property<int?>("CardId")
                        .HasColumnType("int");

                    b.Property<int>("Cost")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<DateTime?>("CreateOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<double?>("Debt")
                        .HasColumnType("float");

                    b.Property<int?>("Duration")
                        .HasColumnType("int");

                    b.Property<double?>("FirstPrice")
                        .HasColumnType("float");

                    b.Property<bool?>("HasBonus")
                        .HasColumnType("bit");

                    b.Property<bool>("IsCredite")
                        .HasColumnType("bit");

                    b.Property<double?>("MonthPrice")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("OldDebt")
                        .HasColumnType("float");

                    b.Property<int?>("PayByBonusCard")
                        .HasColumnType("int");

                    b.Property<DateTime?>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("Price")
                        .HasPrecision(25, 4)
                        .HasColumnType("float(25)");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.Property<string>("Quantity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("StatusNotification")
                        .HasColumnType("int");

                    b.Property<double>("TotalPrice")
                        .HasPrecision(25, 4)
                        .HasColumnType("float(25)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BonusCardId");

                    b.HasIndex("CardId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Parfume.Models.PaymentHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<double>("Debt")
                        .HasColumnType("float");

                    b.Property<double?>("MonthPrice")
                        .HasColumnType("float");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("PayDate")
                        .HasColumnType("datetime2");

                    b.Property<double?>("PayPrice")
                        .HasColumnType("float");

                    b.Property<DateTime?>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Queue")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("OrderId");

                    b.ToTable("PaymentHistories");
                });

            modelBuilder.Entity("Parfume.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Quantity")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Parfume.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Parfume.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("FatherName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fincode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Parfume.Models.UserWebPushCredentials", b =>
                {
                    b.Property<int>("UserWebPushCredentialsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Auth")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("AUTH");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("CREATE_DATE");

                    b.Property<string>("P256dh")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("P256DH");

                    b.Property<string>("PushEndPoint")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("PUSH_END_POINT");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("USER_ID");

                    b.HasKey("UserWebPushCredentialsId");

                    b.HasIndex("UserId");

                    b.ToTable("userWebPushCredentials");
                });

            modelBuilder.Entity("Parfume.Models.Bonus", b =>
                {
                    b.HasOne("Parfume.Models.Customer", "Customer")
                        .WithMany("Bonus")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Parfume.Models.BonusCard", b =>
                {
                    b.HasOne("Parfume.Models.BonusCardType", "BonusCardType")
                        .WithMany("BonusCard")
                        .HasForeignKey("BonusCardTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Parfume.Models.Customer", "Customer")
                        .WithMany("BonusCard")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BonusCardType");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Parfume.Models.BonusCardHistory", b =>
                {
                    b.HasOne("Parfume.Models.BonusCard", "BonusCard")
                        .WithMany("BonusCardHistories")
                        .HasForeignKey("BonusCardId");

                    b.HasOne("Parfume.Models.Customer", "Customer")
                        .WithMany("BonusCardHistories")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Parfume.Models.Order", "Order")
                        .WithMany("BonusCardHistories")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BonusCard");

                    b.Navigation("Customer");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Parfume.Models.BonusHistory", b =>
                {
                    b.HasOne("Parfume.Models.Bonus", "Bonus")
                        .WithMany("BonusHistories")
                        .HasForeignKey("BonusId");

                    b.HasOne("Parfume.Models.Customer", "Customer")
                        .WithMany("BonusHistories")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Parfume.Models.Order", "Order")
                        .WithMany("BonusHistories")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bonus");

                    b.Navigation("Customer");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Parfume.Models.CrediteHistory", b =>
                {
                    b.HasOne("Parfume.Models.Order", "Order")
                        .WithMany("CrediteHistories")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Parfume.Models.PaymentHistory", "PaymentHistory")
                        .WithMany("CrediteHistories")
                        .HasForeignKey("PaymentHistoryId");

                    b.HasOne("Parfume.Models.User", "User")
                        .WithMany("CrediteHistories")
                        .HasForeignKey("UserId");

                    b.Navigation("Order");

                    b.Navigation("PaymentHistory");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Parfume.Models.Customer", b =>
                {
                    b.HasOne("Parfume.Models.Card", "Card")
                        .WithMany("Customers")
                        .HasForeignKey("CardId");

                    b.HasOne("Parfume.Models.Customer", "ReferencesCustomer")
                        .WithMany()
                        .HasForeignKey("ReferencesCustomerId");

                    b.Navigation("Card");

                    b.Navigation("ReferencesCustomer");
                });

            modelBuilder.Entity("Parfume.Models.Log", b =>
                {
                    b.HasOne("Parfume.Models.User", "User")
                        .WithMany("Logs")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Parfume.Models.Order", b =>
                {
                    b.HasOne("Parfume.Models.BonusCard", "BonusCard")
                        .WithMany()
                        .HasForeignKey("BonusCardId");

                    b.HasOne("Parfume.Models.Card", "Card")
                        .WithMany("Orders")
                        .HasForeignKey("CardId");

                    b.HasOne("Parfume.Models.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Parfume.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");

                    b.HasOne("Parfume.Models.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BonusCard");

                    b.Navigation("Card");

                    b.Navigation("Customer");

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Parfume.Models.PaymentHistory", b =>
                {
                    b.HasOne("Parfume.Models.Customer", "Customer")
                        .WithMany("PaymentHistories")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Parfume.Models.Order", "Order")
                        .WithMany("PaymentHistories")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Parfume.Models.User", b =>
                {
                    b.HasOne("Parfume.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Parfume.Models.UserWebPushCredentials", b =>
                {
                    b.HasOne("Parfume.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Parfume.Models.Bonus", b =>
                {
                    b.Navigation("BonusHistories");
                });

            modelBuilder.Entity("Parfume.Models.BonusCard", b =>
                {
                    b.Navigation("BonusCardHistories");
                });

            modelBuilder.Entity("Parfume.Models.BonusCardType", b =>
                {
                    b.Navigation("BonusCard");
                });

            modelBuilder.Entity("Parfume.Models.Card", b =>
                {
                    b.Navigation("Customers");

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Parfume.Models.Customer", b =>
                {
                    b.Navigation("Bonus");

                    b.Navigation("BonusCard");

                    b.Navigation("BonusCardHistories");

                    b.Navigation("BonusHistories");

                    b.Navigation("Orders");

                    b.Navigation("PaymentHistories");
                });

            modelBuilder.Entity("Parfume.Models.Order", b =>
                {
                    b.Navigation("BonusCardHistories");

                    b.Navigation("BonusHistories");

                    b.Navigation("CrediteHistories");

                    b.Navigation("PaymentHistories");
                });

            modelBuilder.Entity("Parfume.Models.PaymentHistory", b =>
                {
                    b.Navigation("CrediteHistories");
                });

            modelBuilder.Entity("Parfume.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Parfume.Models.User", b =>
                {
                    b.Navigation("CrediteHistories");

                    b.Navigation("Logs");

                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
