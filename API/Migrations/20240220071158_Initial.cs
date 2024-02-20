﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsparagusN.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    StreetName = table.Column<string>(type: "TEXT", nullable: false),
                    BuildingName = table.Column<string>(type: "TEXT", nullable: false),
                    ApartmentNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false),
                    Latitude = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdminPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AvailableDate = table.Column<DateTime>(type: "date", nullable: false),
                    PlanType = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Allergies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ArabicName = table.Column<string>(type: "TEXT", nullable: false),
                    EnglishName = table.Column<string>(type: "TEXT", nullable: false),
                    PictureUrl = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allergies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppCoupons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<double>(type: "REAL", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCoupons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NameEN = table.Column<string>(type: "TEXT", nullable: false),
                    NameAR = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerBaskets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerBaskets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Drinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NameArabic = table.Column<string>(type: "TEXT", nullable: false),
                    NameEnglish = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    Volume = table.Column<string>(type: "TEXT", nullable: false),
                    PictureUrl = table.Column<string>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drinks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExtraOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NameArabic = table.Column<string>(type: "TEXT", nullable: false),
                    NameEnglish = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    Weight = table.Column<double>(type: "REAL", nullable: false),
                    PictureUrl = table.Column<string>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    OptionType = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtraOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NameEN = table.Column<string>(type: "TEXT", nullable: false),
                    NameAR = table.Column<string>(type: "TEXT", nullable: false),
                    ExtraInfo = table.Column<string>(type: "TEXT", nullable: false),
                    Weight = table.Column<double>(type: "REAL", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    Protein = table.Column<double>(type: "REAL", nullable: false),
                    Carb = table.Column<double>(type: "REAL", nullable: false),
                    Fat = table.Column<double>(type: "REAL", nullable: false),
                    Fiber = table.Column<double>(type: "REAL", nullable: false),
                    TypeOfIngredient = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    StreetName = table.Column<string>(type: "TEXT", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false),
                    Latitude = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MediaUrls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    IsSplashScreenUrl = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaUrls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlanPrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Duration = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfMealsPerDay = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanPrices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlanTypes",
                columns: table => new
                {
                    PlanTypeE = table.Column<string>(type: "TEXT", nullable: false),
                    Points = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanTypes", x => x.PlanTypeE);
                });

            migrationBuilder.CreateTable(
                name: "UserMealCarb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NameEN = table.Column<string>(type: "TEXT", nullable: false),
                    NameAR = table.Column<string>(type: "TEXT", nullable: false),
                    Protein = table.Column<double>(type: "REAL", nullable: false),
                    Carb = table.Column<double>(type: "REAL", nullable: false),
                    Fat = table.Column<double>(type: "REAL", nullable: false),
                    Fiber = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMealCarb", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Zones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NameAR = table.Column<string>(type: "TEXT", nullable: false),
                    NameEN = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FullName = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    PictureUrl = table.Column<string>(type: "TEXT", nullable: false),
                    Birthday = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Gender = table.Column<string>(type: "TEXT", nullable: false),
                    IsMealPlanMember = table.Column<bool>(type: "INTEGER", nullable: false),
                    LoyaltyPoints = table.Column<int>(type: "INTEGER", nullable: false),
                    HomeAddressId = table.Column<int>(type: "INTEGER", nullable: false),
                    WorkAddressId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsNormalUser = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Address_HomeAddressId",
                        column: x => x.HomeAddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Address_WorkAddressId",
                        column: x => x.WorkAddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<int>(type: "INTEGER", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Meals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NameEN = table.Column<string>(type: "TEXT", nullable: false),
                    NameAR = table.Column<string>(type: "TEXT", nullable: false),
                    DescriptionEN = table.Column<string>(type: "TEXT", nullable: false),
                    DescriptionAR = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<double>(type: "decimal(18,2)", nullable: false),
                    Points = table.Column<int>(type: "INTEGER", nullable: false),
                    PictureUrl = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsMealPlan = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsMainMenu = table.Column<bool>(type: "INTEGER", nullable: false),
                    LoyaltyPoints = table.Column<int>(type: "INTEGER", nullable: true),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    Protein = table.Column<double>(type: "REAL", nullable: false),
                    Carbs = table.Column<double>(type: "REAL", nullable: false),
                    Fats = table.Column<double>(type: "REAL", nullable: false),
                    Fibers = table.Column<double>(type: "REAL", nullable: false),
                    PricePerProtein = table.Column<double>(type: "REAL", nullable: false),
                    PricePerCarb = table.Column<double>(type: "REAL", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Meals_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdminSelectedDrinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DrinkId = table.Column<int>(type: "INTEGER", nullable: false),
                    PlanTypeEnum = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminSelectedDrinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminSelectedDrinks_Drinks_DrinkId",
                        column: x => x.DrinkId,
                        principalTable: "Drinks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdminSelectedExtraOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ExtraOptionId = table.Column<int>(type: "INTEGER", nullable: false),
                    PlanTypeEnum = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminSelectedExtraOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminSelectedExtraOptions_ExtraOptions_ExtraOptionId",
                        column: x => x.ExtraOptionId,
                        principalTable: "ExtraOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdminSelectedCarbs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CarbId = table.Column<int>(type: "INTEGER", nullable: false),
                    PlanTypeEnum = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminSelectedCarbs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminSelectedCarbs_Ingredients_CarbId",
                        column: x => x.CarbId,
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NameEN = table.Column<string>(type: "TEXT", nullable: false),
                    NameAR = table.Column<string>(type: "TEXT", nullable: false),
                    AddressId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Branches_Location_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    ZoneId = table.Column<int>(type: "INTEGER", nullable: false),
                    PictureUrl = table.Column<string>(type: "TEXT", nullable: false),
                    Period = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Drivers_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    RoleId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AppUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Duration = table.Column<int>(type: "INTEGER", nullable: false),
                    PlanType = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfMealPerDay = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfSnacks = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfRemainingSnacks = table.Column<int>(type: "INTEGER", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: true),
                    DeliveryCity = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPlans_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdminSelectedMeals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MealId = table.Column<int>(type: "INTEGER", nullable: false),
                    AdminPlanDayId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminSelectedMeals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminSelectedMeals_AdminPlans_AdminPlanDayId",
                        column: x => x.AdminPlanDayId,
                        principalTable: "AdminPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdminSelectedMeals_Meals_MealId",
                        column: x => x.MealId,
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdminSelectedSnacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SnackId = table.Column<int>(type: "INTEGER", nullable: false),
                    PlanTypeEnum = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminSelectedSnacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminSelectedSnacks_Meals_SnackId",
                        column: x => x.SnackId,
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BasketItems",
                columns: table => new
                {
                    CustomerBasketId = table.Column<int>(type: "INTEGER", nullable: false),
                    MealId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    AddedCarb = table.Column<int>(type: "INTEGER", nullable: false),
                    AddedProtein = table.Column<int>(type: "INTEGER", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: false),
                    RemoveSauce = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasketItems", x => new { x.CustomerBasketId, x.MealId });
                    table.ForeignKey(
                        name: "FK_BasketItems_CustomerBaskets_CustomerBasketId",
                        column: x => x.CustomerBasketId,
                        principalTable: "CustomerBaskets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BasketItems_Meals_MealId",
                        column: x => x.MealId,
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MealAllergies",
                columns: table => new
                {
                    MealId = table.Column<int>(type: "INTEGER", nullable: false),
                    AllergyId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealAllergies", x => new { x.MealId, x.AllergyId });
                    table.ForeignKey(
                        name: "FK_MealAllergies_Allergies_AllergyId",
                        column: x => x.AllergyId,
                        principalTable: "Allergies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MealAllergies_Meals_MealId",
                        column: x => x.MealId,
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MealIngredients",
                columns: table => new
                {
                    MealId = table.Column<int>(type: "INTEGER", nullable: false),
                    IngredientId = table.Column<int>(type: "INTEGER", nullable: false),
                    Weight = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealIngredients", x => new { x.MealId, x.IngredientId });
                    table.ForeignKey(
                        name: "FK_MealIngredients_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MealIngredients_Meals_MealId",
                        column: x => x.MealId,
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BuyerEmail = table.Column<string>(type: "TEXT", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ShipToAddressId = table.Column<int>(type: "INTEGER", nullable: false),
                    BranchId = table.Column<int>(type: "INTEGER", nullable: false),
                    Subtotal = table.Column<double>(type: "REAL", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    PaymentType = table.Column<string>(type: "TEXT", nullable: false),
                    PointsPrice = table.Column<int>(type: "INTEGER", nullable: false),
                    BillId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Address_ShipToAddressId",
                        column: x => x.ShipToAddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPlanAllergy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ArabicName = table.Column<string>(type: "TEXT", nullable: false),
                    EnglishName = table.Column<string>(type: "TEXT", nullable: false),
                    PictureUrl = table.Column<string>(type: "TEXT", nullable: false),
                    UserPlanId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPlanAllergy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPlanAllergy_UserPlans_UserPlanId",
                        column: x => x.UserPlanId,
                        principalTable: "UserPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPlanDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserPlanId = table.Column<int>(type: "INTEGER", nullable: false),
                    Day = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DeliveryLocationId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeliveryPeriod = table.Column<string>(type: "TEXT", nullable: false),
                    DayOrderStatus = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPlanDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPlanDays_Address_DeliveryLocationId",
                        column: x => x.DeliveryLocationId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPlanDays_UserPlans_UserPlanId",
                        column: x => x.UserPlanId,
                        principalTable: "UserPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderedMeal_MealId = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderedMeal_NameEN = table.Column<string>(type: "TEXT", nullable: false),
                    OrderedMeal_NameAR = table.Column<string>(type: "TEXT", nullable: false),
                    OrderedMeal_DescriptionEN = table.Column<string>(type: "TEXT", nullable: false),
                    OrderedMeal_DescriptionAR = table.Column<string>(type: "TEXT", nullable: false),
                    OrderedMeal_PictureUrl = table.Column<string>(type: "TEXT", nullable: false),
                    OrderedMeal_AddedCarb = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderedMeal_AddedProtein = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<double>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    PointsPrice = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSelectedDrinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserPlanDayId = table.Column<int>(type: "INTEGER", nullable: false),
                    NameArabic = table.Column<string>(type: "TEXT", nullable: false),
                    NameEnglish = table.Column<string>(type: "TEXT", nullable: false),
                    Volume = table.Column<int>(type: "INTEGER", nullable: false),
                    PictureUrl = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSelectedDrinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSelectedDrinks_UserPlanDays_UserPlanDayId",
                        column: x => x.UserPlanDayId,
                        principalTable: "UserPlanDays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSelectedExtraOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserPlanDayId = table.Column<int>(type: "INTEGER", nullable: false),
                    NameArabic = table.Column<string>(type: "TEXT", nullable: false),
                    NameEnglish = table.Column<string>(type: "TEXT", nullable: false),
                    Weight = table.Column<double>(type: "REAL", nullable: false),
                    PictureUrl = table.Column<string>(type: "TEXT", nullable: false),
                    OptionType = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSelectedExtraOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSelectedExtraOptions_UserPlanDays_UserPlanDayId",
                        column: x => x.UserPlanDayId,
                        principalTable: "UserPlanDays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSelectedMeals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserPlanDayId = table.Column<int>(type: "INTEGER", nullable: false),
                    NameEN = table.Column<string>(type: "TEXT", nullable: false),
                    NameAR = table.Column<string>(type: "TEXT", nullable: false),
                    DescriptionEN = table.Column<string>(type: "TEXT", nullable: false),
                    DescriptionAR = table.Column<string>(type: "TEXT", nullable: false),
                    PictureUrl = table.Column<string>(type: "TEXT", nullable: false),
                    PricePerProtein = table.Column<double>(type: "REAL", nullable: false),
                    PricePerCarb = table.Column<double>(type: "REAL", nullable: false),
                    Calories = table.Column<double>(type: "REAL", nullable: false),
                    Fibers = table.Column<double>(type: "REAL", nullable: false),
                    Fats = table.Column<double>(type: "REAL", nullable: false),
                    Carbs = table.Column<double>(type: "REAL", nullable: false),
                    Protein = table.Column<double>(type: "REAL", nullable: false),
                    ChangedCarbId = table.Column<int>(type: "INTEGER", nullable: false),
                    OriginalMealId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSelectedMeals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSelectedMeals_UserMealCarb_ChangedCarbId",
                        column: x => x.ChangedCarbId,
                        principalTable: "UserMealCarb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSelectedMeals_UserPlanDays_UserPlanDayId",
                        column: x => x.UserPlanDayId,
                        principalTable: "UserPlanDays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSelectedSnacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NameEN = table.Column<string>(type: "TEXT", nullable: false),
                    NameAR = table.Column<string>(type: "TEXT", nullable: false),
                    DescriptionEN = table.Column<string>(type: "TEXT", nullable: false),
                    DescriptionAR = table.Column<string>(type: "TEXT", nullable: false),
                    PictureUrl = table.Column<string>(type: "TEXT", nullable: false),
                    Protein = table.Column<double>(type: "REAL", nullable: false),
                    Carbs = table.Column<double>(type: "REAL", nullable: false),
                    Fats = table.Column<double>(type: "REAL", nullable: false),
                    Fibers = table.Column<double>(type: "REAL", nullable: false),
                    Calories = table.Column<double>(type: "REAL", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    UserPlanDayId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSelectedSnacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSelectedSnacks_UserPlanDays_UserPlanDayId",
                        column: x => x.UserPlanDayId,
                        principalTable: "UserPlanDays",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminSelectedCarbs_CarbId",
                table: "AdminSelectedCarbs",
                column: "CarbId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminSelectedDrinks_DrinkId",
                table: "AdminSelectedDrinks",
                column: "DrinkId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminSelectedExtraOptions_ExtraOptionId",
                table: "AdminSelectedExtraOptions",
                column: "ExtraOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminSelectedMeals_AdminPlanDayId",
                table: "AdminSelectedMeals",
                column: "AdminPlanDayId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminSelectedMeals_MealId",
                table: "AdminSelectedMeals",
                column: "MealId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminSelectedSnacks_SnackId",
                table: "AdminSelectedSnacks",
                column: "SnackId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_HomeAddressId",
                table: "AspNetUsers",
                column: "HomeAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_WorkAddressId",
                table: "AspNetUsers",
                column: "WorkAddressId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BasketItems_MealId",
                table: "BasketItems",
                column: "MealId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_AddressId",
                table: "Branches",
                column: "AddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_ZoneId",
                table: "Drivers",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_MealAllergies_AllergyId",
                table: "MealAllergies",
                column: "AllergyId");

            migrationBuilder.CreateIndex(
                name: "IX_MealIngredients_IngredientId",
                table: "MealIngredients",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_Meals_CategoryId",
                table: "Meals",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BranchId",
                table: "Orders",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShipToAddressId",
                table: "Orders",
                column: "ShipToAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlanAllergy_UserPlanId",
                table: "UserPlanAllergy",
                column: "UserPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlanDays_DeliveryLocationId",
                table: "UserPlanDays",
                column: "DeliveryLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlanDays_UserPlanId",
                table: "UserPlanDays",
                column: "UserPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlans_AppUserId",
                table: "UserPlans",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSelectedDrinks_UserPlanDayId",
                table: "UserSelectedDrinks",
                column: "UserPlanDayId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSelectedExtraOptions_UserPlanDayId",
                table: "UserSelectedExtraOptions",
                column: "UserPlanDayId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSelectedMeals_ChangedCarbId",
                table: "UserSelectedMeals",
                column: "ChangedCarbId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSelectedMeals_UserPlanDayId",
                table: "UserSelectedMeals",
                column: "UserPlanDayId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSelectedSnacks_UserPlanDayId",
                table: "UserSelectedSnacks",
                column: "UserPlanDayId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminSelectedCarbs");

            migrationBuilder.DropTable(
                name: "AdminSelectedDrinks");

            migrationBuilder.DropTable(
                name: "AdminSelectedExtraOptions");

            migrationBuilder.DropTable(
                name: "AdminSelectedMeals");

            migrationBuilder.DropTable(
                name: "AdminSelectedSnacks");

            migrationBuilder.DropTable(
                name: "AppCoupons");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BasketItems");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "MealAllergies");

            migrationBuilder.DropTable(
                name: "MealIngredients");

            migrationBuilder.DropTable(
                name: "MediaUrls");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "PlanPrices");

            migrationBuilder.DropTable(
                name: "PlanTypes");

            migrationBuilder.DropTable(
                name: "UserPlanAllergy");

            migrationBuilder.DropTable(
                name: "UserSelectedDrinks");

            migrationBuilder.DropTable(
                name: "UserSelectedExtraOptions");

            migrationBuilder.DropTable(
                name: "UserSelectedMeals");

            migrationBuilder.DropTable(
                name: "UserSelectedSnacks");

            migrationBuilder.DropTable(
                name: "Drinks");

            migrationBuilder.DropTable(
                name: "ExtraOptions");

            migrationBuilder.DropTable(
                name: "AdminPlans");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "CustomerBaskets");

            migrationBuilder.DropTable(
                name: "Zones");

            migrationBuilder.DropTable(
                name: "Allergies");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "Meals");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "UserMealCarb");

            migrationBuilder.DropTable(
                name: "UserPlanDays");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "UserPlans");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Address");
        }
    }
}
