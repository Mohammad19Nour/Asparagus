CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

BEGIN TRANSACTION;

CREATE TABLE "Address" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Address" PRIMARY KEY AUTOINCREMENT,
    "City" TEXT NOT NULL,
    "StreetName" TEXT NOT NULL,
    "BuildingName" TEXT NOT NULL,
    "ApartmentNumber" INTEGER NOT NULL,
    "Longitude" REAL NOT NULL,
    "Latitude" REAL NOT NULL
);

CREATE TABLE "AdminPlans" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_AdminPlans" PRIMARY KEY AUTOINCREMENT,
    "AvailableDate" date NOT NULL,
    "PlanType" TEXT NOT NULL
);

CREATE TABLE "Allergies" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Allergies" PRIMARY KEY AUTOINCREMENT,
    "ArabicName" TEXT NOT NULL,
    "EnglishName" TEXT NOT NULL,
    "PictureUrl" TEXT NOT NULL
);

CREATE TABLE "AppCoupons" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_AppCoupons" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Code" TEXT NOT NULL,
    "Value" REAL NOT NULL,
    "Type" TEXT NOT NULL
);

CREATE TABLE "AspNetRoles" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_AspNetRoles" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NULL,
    "NormalizedName" TEXT NULL,
    "ConcurrencyStamp" TEXT NULL
);

CREATE TABLE "Categories" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Categories" PRIMARY KEY AUTOINCREMENT,
    "NameEN" TEXT NOT NULL,
    "NameAR" TEXT NOT NULL,
    "Description" TEXT NOT NULL
);

CREATE TABLE "CustomerBaskets" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_CustomerBaskets" PRIMARY KEY
);

CREATE TABLE "Drinks" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Drinks" PRIMARY KEY AUTOINCREMENT,
    "NameArabic" TEXT NOT NULL,
    "NameEnglish" TEXT NOT NULL,
    "Price" REAL NOT NULL,
    "Volume" TEXT NOT NULL,
    "PictureUrl" TEXT NOT NULL,
    "IsDeleted" INTEGER NOT NULL
);

CREATE TABLE "ExtraOptions" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_ExtraOptions" PRIMARY KEY AUTOINCREMENT,
    "NameArabic" TEXT NOT NULL,
    "NameEnglish" TEXT NOT NULL,
    "Price" REAL NOT NULL,
    "Weight" REAL NOT NULL,
    "PictureUrl" TEXT NOT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "OptionType" TEXT NOT NULL
);

CREATE TABLE "Ingredients" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Ingredients" PRIMARY KEY AUTOINCREMENT,
    "NameEN" TEXT NOT NULL,
    "NameAR" TEXT NOT NULL,
    "ExtraInfo" TEXT NOT NULL,
    "Weight" REAL NOT NULL,
    "Price" REAL NOT NULL,
    "Protein" REAL NOT NULL,
    "Carb" REAL NOT NULL,
    "Fat" REAL NOT NULL,
    "Fiber" REAL NOT NULL,
    "TypeOfIngredient" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "IsDeleted" INTEGER NOT NULL
);

CREATE TABLE "Location" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Location" PRIMARY KEY AUTOINCREMENT,
    "City" TEXT NOT NULL,
    "StreetName" TEXT NOT NULL,
    "Longitude" REAL NOT NULL,
    "Latitude" REAL NOT NULL
);

CREATE TABLE "MediaUrls" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_MediaUrls" PRIMARY KEY AUTOINCREMENT,
    "Url" TEXT NOT NULL,
    "IsSplashScreenUrl" INTEGER NOT NULL
);

CREATE TABLE "Notifications" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Notifications" PRIMARY KEY AUTOINCREMENT,
    "UserEmail" TEXT NOT NULL,
    "ArabicContent" TEXT NOT NULL,
    "EnglishContent" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "IsSent" INTEGER NOT NULL
);

CREATE TABLE "PlanPrices" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_PlanPrices" PRIMARY KEY AUTOINCREMENT,
    "Duration" INTEGER NOT NULL,
    "NumberOfMealsPerDay" INTEGER NOT NULL,
    "Price" REAL NOT NULL
);

CREATE TABLE "PlanTypes" (
    "PlanTypeE" TEXT NOT NULL CONSTRAINT "PK_PlanTypes" PRIMARY KEY,
    "Points" INTEGER NOT NULL
);

CREATE TABLE "Questions" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Questions" PRIMARY KEY AUTOINCREMENT,
    "Title" TEXT NOT NULL,
    "ParentFAQId" INTEGER NULL,
    CONSTRAINT "FK_Questions_Questions_ParentFAQId" FOREIGN KEY ("ParentFAQId") REFERENCES "Questions" ("Id")
);

CREATE TABLE "UserMealCarb" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_UserMealCarb" PRIMARY KEY AUTOINCREMENT,
    "NameEN" TEXT NOT NULL,
    "NameAR" TEXT NOT NULL,
    "Protein" REAL NOT NULL,
    "Carb" REAL NOT NULL,
    "Fat" REAL NOT NULL,
    "Fiber" REAL NOT NULL
);

CREATE TABLE "Zones" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Zones" PRIMARY KEY AUTOINCREMENT,
    "NameAR" TEXT NOT NULL,
    "NameEN" TEXT NOT NULL
);

CREATE TABLE "AspNetUsers" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_AspNetUsers" PRIMARY KEY AUTOINCREMENT,
    "FullName" TEXT NOT NULL,
    "Email" TEXT NOT NULL,
    "PhoneNumber" TEXT NOT NULL,
    "PictureUrl" TEXT NOT NULL,
    "Birthday" TEXT NOT NULL,
    "RegistrationDate" TEXT NOT NULL,
    "Gender" TEXT NOT NULL,
    "IsMealPlanMember" INTEGER NOT NULL,
    "LoyaltyPoints" INTEGER NOT NULL,
    "HomeAddressId" INTEGER NOT NULL,
    "WorkAddressId" INTEGER NOT NULL,
    "IsNormalUser" INTEGER NOT NULL,
    "UserName" TEXT NULL,
    "NormalizedUserName" TEXT NULL,
    "NormalizedEmail" TEXT NULL,
    "EmailConfirmed" INTEGER NOT NULL,
    "PasswordHash" TEXT NULL,
    "SecurityStamp" TEXT NULL,
    "ConcurrencyStamp" TEXT NULL,
    "PhoneNumberConfirmed" INTEGER NOT NULL,
    "TwoFactorEnabled" INTEGER NOT NULL,
    "LockoutEnd" TEXT NULL,
    "LockoutEnabled" INTEGER NOT NULL,
    "AccessFailedCount" INTEGER NOT NULL,
    CONSTRAINT "FK_AspNetUsers_Address_HomeAddressId" FOREIGN KEY ("HomeAddressId") REFERENCES "Address" ("Id"),
    CONSTRAINT "FK_AspNetUsers_Address_WorkAddressId" FOREIGN KEY ("WorkAddressId") REFERENCES "Address" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetRoleClaims" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_AspNetRoleClaims" PRIMARY KEY AUTOINCREMENT,
    "RoleId" INTEGER NOT NULL,
    "ClaimType" TEXT NULL,
    "ClaimValue" TEXT NULL,
    CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Meals" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Meals" PRIMARY KEY AUTOINCREMENT,
    "NameEN" TEXT NOT NULL,
    "NameAR" TEXT NOT NULL,
    "DescriptionEN" TEXT NOT NULL,
    "DescriptionAR" TEXT NOT NULL,
    "Price" decimal(18,2) NOT NULL,
    "Points" INTEGER NOT NULL,
    "PictureUrl" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "IsMealPlan" INTEGER NOT NULL,
    "IsMainMenu" INTEGER NOT NULL,
    "LoyaltyPoints" INTEGER NULL,
    "CategoryId" INTEGER NOT NULL,
    "Protein" REAL NOT NULL,
    "Carbs" REAL NOT NULL,
    "Fats" REAL NOT NULL,
    "Fibers" REAL NOT NULL,
    "PricePerProtein" REAL NOT NULL,
    "PricePerCarb" REAL NOT NULL,
    "IsDeleted" INTEGER NOT NULL,
    CONSTRAINT "FK_Meals_Categories_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES "Categories" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AdminSelectedDrinks" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_AdminSelectedDrinks" PRIMARY KEY AUTOINCREMENT,
    "DrinkId" INTEGER NOT NULL,
    "PlanTypeEnum" INTEGER NOT NULL,
    CONSTRAINT "FK_AdminSelectedDrinks_Drinks_DrinkId" FOREIGN KEY ("DrinkId") REFERENCES "Drinks" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AdminSelectedExtraOptions" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_AdminSelectedExtraOptions" PRIMARY KEY AUTOINCREMENT,
    "ExtraOptionId" INTEGER NOT NULL,
    "PlanTypeEnum" INTEGER NOT NULL,
    CONSTRAINT "FK_AdminSelectedExtraOptions_ExtraOptions_ExtraOptionId" FOREIGN KEY ("ExtraOptionId") REFERENCES "ExtraOptions" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AdminSelectedCarbs" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_AdminSelectedCarbs" PRIMARY KEY AUTOINCREMENT,
    "CarbId" INTEGER NOT NULL,
    "PlanTypeEnum" INTEGER NOT NULL,
    CONSTRAINT "FK_AdminSelectedCarbs_Ingredients_CarbId" FOREIGN KEY ("CarbId") REFERENCES "Ingredients" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Branches" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Branches" PRIMARY KEY AUTOINCREMENT,
    "NameEN" TEXT NOT NULL,
    "NameAR" TEXT NOT NULL,
    "AddressId" INTEGER NOT NULL,
    CONSTRAINT "FK_Branches_Location_AddressId" FOREIGN KEY ("AddressId") REFERENCES "Location" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Drivers" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Drivers" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "PhoneNumber" TEXT NOT NULL,
    "Email" TEXT NOT NULL,
    "Password" TEXT NOT NULL,
    "IsActive" INTEGER NOT NULL,
    "ZoneId" INTEGER NOT NULL,
    "PictureUrl" TEXT NOT NULL,
    "Period" TEXT NOT NULL,
    "RegistrationDate" TEXT NOT NULL,
    "Status" TEXT NOT NULL,
    CONSTRAINT "FK_Drivers_Zones_ZoneId" FOREIGN KEY ("ZoneId") REFERENCES "Zones" ("Id") ON DELETE RESTRICT
);

CREATE TABLE "AspNetUserClaims" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY AUTOINCREMENT,
    "UserId" INTEGER NOT NULL,
    "ClaimType" TEXT NULL,
    "ClaimValue" TEXT NULL,
    CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserLogins" (
    "LoginProvider" TEXT NOT NULL,
    "ProviderKey" TEXT NOT NULL,
    "ProviderDisplayName" TEXT NULL,
    "UserId" INTEGER NOT NULL,
    CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey"),
    CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserRoles" (
    "UserId" INTEGER NOT NULL,
    "RoleId" INTEGER NOT NULL,
    CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserTokens" (
    "UserId" INTEGER NOT NULL,
    "LoginProvider" TEXT NOT NULL,
    "Name" TEXT NOT NULL,
    "Value" TEXT NULL,
    CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
    CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "UserPlans" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_UserPlans" PRIMARY KEY AUTOINCREMENT,
    "AppUserId" INTEGER NOT NULL,
    "Price" REAL NOT NULL,
    "CreatedDate" TEXT NOT NULL,
    "StartDate" TEXT NOT NULL,
    "Duration" INTEGER NOT NULL,
    "PlanType" INTEGER NOT NULL,
    "NumberOfMealPerDay" INTEGER NOT NULL,
    "NumberOfSnacks" INTEGER NOT NULL,
    "NumberOfRemainingSnacks" INTEGER NOT NULL,
    "Notes" TEXT NULL,
    "DeliveryCity" TEXT NOT NULL,
    CONSTRAINT "FK_UserPlans_AspNetUsers_AppUserId" FOREIGN KEY ("AppUserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AdminSelectedMeals" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_AdminSelectedMeals" PRIMARY KEY AUTOINCREMENT,
    "MealId" INTEGER NOT NULL,
    "AdminPlanDayId" INTEGER NOT NULL,
    CONSTRAINT "FK_AdminSelectedMeals_AdminPlans_AdminPlanDayId" FOREIGN KEY ("AdminPlanDayId") REFERENCES "AdminPlans" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_AdminSelectedMeals_Meals_MealId" FOREIGN KEY ("MealId") REFERENCES "Meals" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AdminSelectedSnacks" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_AdminSelectedSnacks" PRIMARY KEY AUTOINCREMENT,
    "SnackId" INTEGER NOT NULL,
    "PlanTypeEnum" INTEGER NOT NULL,
    CONSTRAINT "FK_AdminSelectedSnacks_Meals_SnackId" FOREIGN KEY ("SnackId") REFERENCES "Meals" ("Id") ON DELETE CASCADE
);

CREATE TABLE "BasketItems" (
    "CustomerBasketId" INTEGER NOT NULL,
    "MealId" INTEGER NOT NULL,
    "Quantity" INTEGER NOT NULL,
    "AddedCarb" INTEGER NOT NULL,
    "AddedProtein" INTEGER NOT NULL,
    "Note" TEXT NOT NULL,
    "RemoveSauce" INTEGER NOT NULL,
    CONSTRAINT "PK_BasketItems" PRIMARY KEY ("CustomerBasketId", "MealId"),
    CONSTRAINT "FK_BasketItems_CustomerBaskets_CustomerBasketId" FOREIGN KEY ("CustomerBasketId") REFERENCES "CustomerBaskets" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_BasketItems_Meals_MealId" FOREIGN KEY ("MealId") REFERENCES "Meals" ("Id") ON DELETE CASCADE
);

CREATE TABLE "GiftSelections" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_GiftSelections" PRIMARY KEY AUTOINCREMENT,
    "Month" INTEGER NOT NULL,
    "MonthName" TEXT NOT NULL,
    "MealId" INTEGER NULL,
    CONSTRAINT "FK_GiftSelections_Meals_MealId" FOREIGN KEY ("MealId") REFERENCES "Meals" ("Id") ON DELETE SET NULL
);

CREATE TABLE "MealAllergies" (
    "MealId" INTEGER NOT NULL,
    "AllergyId" INTEGER NOT NULL,
    CONSTRAINT "PK_MealAllergies" PRIMARY KEY ("MealId", "AllergyId"),
    CONSTRAINT "FK_MealAllergies_Allergies_AllergyId" FOREIGN KEY ("AllergyId") REFERENCES "Allergies" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_MealAllergies_Meals_MealId" FOREIGN KEY ("MealId") REFERENCES "Meals" ("Id") ON DELETE CASCADE
);

CREATE TABLE "MealIngredients" (
    "MealId" INTEGER NOT NULL,
    "IngredientId" INTEGER NOT NULL,
    "Weight" REAL NOT NULL,
    CONSTRAINT "PK_MealIngredients" PRIMARY KEY ("MealId", "IngredientId"),
    CONSTRAINT "FK_MealIngredients_Ingredients_IngredientId" FOREIGN KEY ("IngredientId") REFERENCES "Ingredients" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_MealIngredients_Meals_MealId" FOREIGN KEY ("MealId") REFERENCES "Meals" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Cashiers" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Cashiers" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "PhoneNumber" TEXT NOT NULL,
    "Email" TEXT NOT NULL,
    "Password" TEXT NOT NULL,
    "IsActive" INTEGER NOT NULL,
    "BranchId" INTEGER NOT NULL,
    "PictureUrl" TEXT NOT NULL,
    "Period" INTEGER NOT NULL,
    CONSTRAINT "FK_Cashiers_Branches_BranchId" FOREIGN KEY ("BranchId") REFERENCES "Branches" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Orders" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Orders" PRIMARY KEY AUTOINCREMENT,
    "BuyerEmail" TEXT NOT NULL,
    "BuyerId" INTEGER NOT NULL,
    "BuyerPhoneNumber" TEXT NOT NULL,
    "OrderDate" TEXT NOT NULL,
    "ShipToAddressId" INTEGER NOT NULL,
    "BranchId" INTEGER NOT NULL,
    "Subtotal" REAL NOT NULL,
    "Status" TEXT NOT NULL,
    "PaymentType" TEXT NOT NULL,
    "PointsPrice" INTEGER NOT NULL,
    "BillId" TEXT NULL,
    "DriverId" INTEGER NULL,
    "GainedPoints" INTEGER NOT NULL,
    "CouponValue" REAL NOT NULL,
    "Priority" INTEGER NULL,
    CONSTRAINT "FK_Orders_Address_ShipToAddressId" FOREIGN KEY ("ShipToAddressId") REFERENCES "Address" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Orders_Branches_BranchId" FOREIGN KEY ("BranchId") REFERENCES "Branches" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Orders_Drivers_DriverId" FOREIGN KEY ("DriverId") REFERENCES "Drivers" ("Id") ON DELETE SET NULL
);

CREATE TABLE "UserPlanAllergy" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_UserPlanAllergy" PRIMARY KEY AUTOINCREMENT,
    "ArabicName" TEXT NOT NULL,
    "EnglishName" TEXT NOT NULL,
    "PictureUrl" TEXT NOT NULL,
    "UserPlanId" INTEGER NULL,
    CONSTRAINT "FK_UserPlanAllergy_UserPlans_UserPlanId" FOREIGN KEY ("UserPlanId") REFERENCES "UserPlans" ("Id") ON DELETE CASCADE
);

CREATE TABLE "UserPlanDays" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_UserPlanDays" PRIMARY KEY AUTOINCREMENT,
    "UserPlanId" INTEGER NOT NULL,
    "Day" TEXT NOT NULL,
    "DeliveryLocationId" INTEGER NOT NULL,
    "DeliveryPeriod" TEXT NOT NULL,
    "IsHomeAddress" INTEGER NOT NULL,
    "DayOrderStatus" INTEGER NOT NULL,
    "IsCustomerInfoPrinted" INTEGER NOT NULL,
    "IsMealsInfoPrinted" INTEGER NOT NULL,
    CONSTRAINT "FK_UserPlanDays_Address_DeliveryLocationId" FOREIGN KEY ("DeliveryLocationId") REFERENCES "Address" ("Id"),
    CONSTRAINT "FK_UserPlanDays_UserPlans_UserPlanId" FOREIGN KEY ("UserPlanId") REFERENCES "UserPlans" ("Id") ON DELETE CASCADE
);

CREATE TABLE "OrderItems" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_OrderItems" PRIMARY KEY,
    "OrderedMeal_MealId" INTEGER NOT NULL,
    "OrderedMeal_NameEN" TEXT NOT NULL,
    "OrderedMeal_NameAR" TEXT NOT NULL,
    "OrderedMeal_DescriptionEN" TEXT NOT NULL,
    "OrderedMeal_DescriptionAR" TEXT NOT NULL,
    "OrderedMeal_PictureUrl" TEXT NOT NULL,
    "OrderedMeal_PricePerProtein" REAL NOT NULL,
    "OrderedMeal_PricePerCarb" REAL NOT NULL,
    "OrderedMeal_Calories" REAL NOT NULL,
    "OrderedMeal_Fibers" REAL NOT NULL,
    "OrderedMeal_Fats" REAL NOT NULL,
    "OrderedMeal_Carbs" REAL NOT NULL,
    "OrderedMeal_Protein" REAL NOT NULL,
    "OrderedMeal_AddedCarb" INTEGER NOT NULL,
    "OrderedMeal_AddedProtein" INTEGER NOT NULL,
    "Price" decimal(18,2) NOT NULL,
    "Quantity" INTEGER NOT NULL,
    "PointsPrice" INTEGER NOT NULL,
    "GainedPoint" INTEGER NOT NULL,
    "OrderId" INTEGER NULL,
    CONSTRAINT "FK_OrderItems_Orders_OrderId" FOREIGN KEY ("OrderId") REFERENCES "Orders" ("Id") ON DELETE CASCADE
);

CREATE TABLE "UserSelectedDrinks" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_UserSelectedDrinks" PRIMARY KEY AUTOINCREMENT,
    "UserPlanDayId" INTEGER NOT NULL,
    "NameArabic" TEXT NOT NULL,
    "NameEnglish" TEXT NOT NULL,
    "Volume" INTEGER NOT NULL,
    "PictureUrl" TEXT NOT NULL,
    "Price" REAL NOT NULL,
    CONSTRAINT "FK_UserSelectedDrinks_UserPlanDays_UserPlanDayId" FOREIGN KEY ("UserPlanDayId") REFERENCES "UserPlanDays" ("Id") ON DELETE CASCADE
);

CREATE TABLE "UserSelectedExtraOptions" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_UserSelectedExtraOptions" PRIMARY KEY AUTOINCREMENT,
    "UserPlanDayId" INTEGER NOT NULL,
    "NameArabic" TEXT NOT NULL,
    "NameEnglish" TEXT NOT NULL,
    "Weight" REAL NOT NULL,
    "PictureUrl" TEXT NOT NULL,
    "OptionType" INTEGER NOT NULL,
    "Price" REAL NOT NULL,
    CONSTRAINT "FK_UserSelectedExtraOptions_UserPlanDays_UserPlanDayId" FOREIGN KEY ("UserPlanDayId") REFERENCES "UserPlanDays" ("Id") ON DELETE CASCADE
);

CREATE TABLE "UserSelectedMeals" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_UserSelectedMeals" PRIMARY KEY AUTOINCREMENT,
    "UserPlanDayId" INTEGER NOT NULL,
    "NameEN" TEXT NOT NULL,
    "NameAR" TEXT NOT NULL,
    "DescriptionEN" TEXT NOT NULL,
    "DescriptionAR" TEXT NOT NULL,
    "PictureUrl" TEXT NOT NULL,
    "PricePerProtein" REAL NOT NULL,
    "PricePerCarb" REAL NOT NULL,
    "Calories" REAL NOT NULL,
    "Fibers" REAL NOT NULL,
    "Fats" REAL NOT NULL,
    "Carbs" REAL NOT NULL,
    "Protein" REAL NOT NULL,
    "ChangedCarbId" INTEGER NOT NULL,
    "OriginalMealId" INTEGER NOT NULL,
    CONSTRAINT "FK_UserSelectedMeals_UserMealCarb_ChangedCarbId" FOREIGN KEY ("ChangedCarbId") REFERENCES "UserMealCarb" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_UserSelectedMeals_UserPlanDays_UserPlanDayId" FOREIGN KEY ("UserPlanDayId") REFERENCES "UserPlanDays" ("Id") ON DELETE CASCADE
);

CREATE TABLE "UserSelectedSnacks" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_UserSelectedSnacks" PRIMARY KEY AUTOINCREMENT,
    "NameEN" TEXT NOT NULL,
    "NameAR" TEXT NOT NULL,
    "DescriptionEN" TEXT NOT NULL,
    "DescriptionAR" TEXT NOT NULL,
    "PictureUrl" TEXT NOT NULL,
    "Protein" REAL NOT NULL,
    "Carbs" REAL NOT NULL,
    "Fats" REAL NOT NULL,
    "Fibers" REAL NOT NULL,
    "Calories" REAL NOT NULL,
    "Quantity" INTEGER NOT NULL,
    "UserPlanDayId" INTEGER NULL,
    CONSTRAINT "FK_UserSelectedSnacks_UserPlanDays_UserPlanDayId" FOREIGN KEY ("UserPlanDayId") REFERENCES "UserPlanDays" ("Id")
);

CREATE INDEX "IX_AdminSelectedCarbs_CarbId" ON "AdminSelectedCarbs" ("CarbId");

CREATE INDEX "IX_AdminSelectedDrinks_DrinkId" ON "AdminSelectedDrinks" ("DrinkId");

CREATE INDEX "IX_AdminSelectedExtraOptions_ExtraOptionId" ON "AdminSelectedExtraOptions" ("ExtraOptionId");

CREATE INDEX "IX_AdminSelectedMeals_AdminPlanDayId" ON "AdminSelectedMeals" ("AdminPlanDayId");

CREATE INDEX "IX_AdminSelectedMeals_MealId" ON "AdminSelectedMeals" ("MealId");

CREATE INDEX "IX_AdminSelectedSnacks_SnackId" ON "AdminSelectedSnacks" ("SnackId");

CREATE INDEX "IX_AspNetRoleClaims_RoleId" ON "AspNetRoleClaims" ("RoleId");

CREATE UNIQUE INDEX "RoleNameIndex" ON "AspNetRoles" ("NormalizedName");

CREATE INDEX "IX_AspNetUserClaims_UserId" ON "AspNetUserClaims" ("UserId");

CREATE INDEX "IX_AspNetUserLogins_UserId" ON "AspNetUserLogins" ("UserId");

CREATE INDEX "IX_AspNetUserRoles_RoleId" ON "AspNetUserRoles" ("RoleId");

CREATE INDEX "EmailIndex" ON "AspNetUsers" ("NormalizedEmail");

CREATE INDEX "IX_AspNetUsers_HomeAddressId" ON "AspNetUsers" ("HomeAddressId");

CREATE INDEX "IX_AspNetUsers_WorkAddressId" ON "AspNetUsers" ("WorkAddressId");

CREATE UNIQUE INDEX "UserNameIndex" ON "AspNetUsers" ("NormalizedUserName");

CREATE INDEX "IX_BasketItems_MealId" ON "BasketItems" ("MealId");

CREATE UNIQUE INDEX "IX_Branches_AddressId" ON "Branches" ("AddressId");

CREATE INDEX "IX_Cashiers_BranchId" ON "Cashiers" ("BranchId");

CREATE INDEX "IX_Drivers_ZoneId" ON "Drivers" ("ZoneId");

CREATE INDEX "IX_GiftSelections_MealId" ON "GiftSelections" ("MealId");

CREATE INDEX "IX_MealAllergies_AllergyId" ON "MealAllergies" ("AllergyId");

CREATE INDEX "IX_MealIngredients_IngredientId" ON "MealIngredients" ("IngredientId");

CREATE INDEX "IX_Meals_CategoryId" ON "Meals" ("CategoryId");

CREATE INDEX "IX_OrderItems_OrderId" ON "OrderItems" ("OrderId");

CREATE INDEX "IX_Orders_BranchId" ON "Orders" ("BranchId");

CREATE INDEX "IX_Orders_DriverId" ON "Orders" ("DriverId");

CREATE INDEX "IX_Orders_ShipToAddressId" ON "Orders" ("ShipToAddressId");

CREATE INDEX "IX_Questions_ParentFAQId" ON "Questions" ("ParentFAQId");

CREATE INDEX "IX_UserPlanAllergy_UserPlanId" ON "UserPlanAllergy" ("UserPlanId");

CREATE INDEX "IX_UserPlanDays_DeliveryLocationId" ON "UserPlanDays" ("DeliveryLocationId");

CREATE INDEX "IX_UserPlanDays_UserPlanId" ON "UserPlanDays" ("UserPlanId");

CREATE INDEX "IX_UserPlans_AppUserId" ON "UserPlans" ("AppUserId");

CREATE INDEX "IX_UserSelectedDrinks_UserPlanDayId" ON "UserSelectedDrinks" ("UserPlanDayId");

CREATE INDEX "IX_UserSelectedExtraOptions_UserPlanDayId" ON "UserSelectedExtraOptions" ("UserPlanDayId");

CREATE INDEX "IX_UserSelectedMeals_ChangedCarbId" ON "UserSelectedMeals" ("ChangedCarbId");

CREATE INDEX "IX_UserSelectedMeals_UserPlanDayId" ON "UserSelectedMeals" ("UserPlanDayId");

CREATE INDEX "IX_UserSelectedSnacks_UserPlanDayId" ON "UserSelectedSnacks" ("UserPlanDayId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240227164643_Initial', '7.0.4');

COMMIT;

BEGIN TRANSACTION;

CREATE TABLE "ef_temp_Questions" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Questions" PRIMARY KEY AUTOINCREMENT,
    "ParentFAQId" INTEGER NULL,
    "Title" TEXT NOT NULL,
    CONSTRAINT "FK_Questions_Questions_ParentFAQId" FOREIGN KEY ("ParentFAQId") REFERENCES "Questions" ("Id") ON DELETE CASCADE
);

INSERT INTO "ef_temp_Questions" ("Id", "ParentFAQId", "Title")
SELECT "Id", "ParentFAQId", "Title"
FROM "Questions";

CREATE TABLE "ef_temp_OrderItems" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_OrderItems" PRIMARY KEY AUTOINCREMENT,
    "GainedPoint" INTEGER NOT NULL,
    "OrderId" INTEGER NULL,
    "OrderedMeal_AddedCarb" INTEGER NOT NULL,
    "OrderedMeal_AddedProtein" INTEGER NOT NULL,
    "OrderedMeal_Calories" REAL NOT NULL,
    "OrderedMeal_Carbs" REAL NOT NULL,
    "OrderedMeal_DescriptionAR" TEXT NOT NULL,
    "OrderedMeal_DescriptionEN" TEXT NOT NULL,
    "OrderedMeal_Fats" REAL NOT NULL,
    "OrderedMeal_Fibers" REAL NOT NULL,
    "OrderedMeal_MealId" INTEGER NOT NULL,
    "OrderedMeal_NameAR" TEXT NOT NULL,
    "OrderedMeal_NameEN" TEXT NOT NULL,
    "OrderedMeal_PictureUrl" TEXT NOT NULL,
    "OrderedMeal_PricePerCarb" REAL NOT NULL,
    "OrderedMeal_PricePerProtein" REAL NOT NULL,
    "OrderedMeal_Protein" REAL NOT NULL,
    "PointsPrice" INTEGER NOT NULL,
    "Price" decimal(18,2) NOT NULL,
    "Quantity" INTEGER NOT NULL,
    CONSTRAINT "FK_OrderItems_Orders_OrderId" FOREIGN KEY ("OrderId") REFERENCES "Orders" ("Id") ON DELETE CASCADE
);

INSERT INTO "ef_temp_OrderItems" ("Id", "GainedPoint", "OrderId", "OrderedMeal_AddedCarb", "OrderedMeal_AddedProtein", "OrderedMeal_Calories", "OrderedMeal_Carbs", "OrderedMeal_DescriptionAR", "OrderedMeal_DescriptionEN", "OrderedMeal_Fats", "OrderedMeal_Fibers", "OrderedMeal_MealId", "OrderedMeal_NameAR", "OrderedMeal_NameEN", "OrderedMeal_PictureUrl", "OrderedMeal_PricePerCarb", "OrderedMeal_PricePerProtein", "OrderedMeal_Protein", "PointsPrice", "Price", "Quantity")
SELECT "Id", "GainedPoint", "OrderId", "OrderedMeal_AddedCarb", "OrderedMeal_AddedProtein", "OrderedMeal_Calories", "OrderedMeal_Carbs", "OrderedMeal_DescriptionAR", "OrderedMeal_DescriptionEN", "OrderedMeal_Fats", "OrderedMeal_Fibers", "OrderedMeal_MealId", "OrderedMeal_NameAR", "OrderedMeal_NameEN", "OrderedMeal_PictureUrl", "OrderedMeal_PricePerCarb", "OrderedMeal_PricePerProtein", "OrderedMeal_Protein", "PointsPrice", "Price", "Quantity"
FROM "OrderItems";

COMMIT;

PRAGMA foreign_keys = 0;

BEGIN TRANSACTION;

DROP TABLE "Questions";

ALTER TABLE "ef_temp_Questions" RENAME TO "Questions";

DROP TABLE "OrderItems";

ALTER TABLE "ef_temp_OrderItems" RENAME TO "OrderItems";

COMMIT;

PRAGMA foreign_keys = 1;

BEGIN TRANSACTION;

CREATE INDEX "IX_Questions_ParentFAQId" ON "Questions" ("ParentFAQId");

CREATE INDEX "IX_OrderItems_OrderId" ON "OrderItems" ("OrderId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240228034350_conf', '7.0.4');

COMMIT;

BEGIN TRANSACTION;

ALTER TABLE "Questions" ADD "ParentFAQId1" INTEGER NULL;

CREATE INDEX "IX_Questions_ParentFAQId1" ON "Questions" ("ParentFAQId1");

CREATE TABLE "ef_temp_Questions" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Questions" PRIMARY KEY AUTOINCREMENT,
    "ParentFAQId" INTEGER NOT NULL,
    "ParentFAQId1" INTEGER NULL,
    "Title" TEXT NOT NULL,
    CONSTRAINT "FK_Questions_Questions_ParentFAQId" FOREIGN KEY ("ParentFAQId") REFERENCES "Questions" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Questions_Questions_ParentFAQId1" FOREIGN KEY ("ParentFAQId1") REFERENCES "Questions" ("Id")
);

INSERT INTO "ef_temp_Questions" ("Id", "ParentFAQId", "ParentFAQId1", "Title")
SELECT "Id", IFNULL("ParentFAQId", 0), "ParentFAQId1", "Title"
FROM "Questions";

CREATE TABLE "ef_temp_OrderItems" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_OrderItems" PRIMARY KEY AUTOINCREMENT,
    "GainedPoint" INTEGER NOT NULL,
    "OrderId" INTEGER NULL,
    "OrderedMeal_AddedCarb" INTEGER NOT NULL,
    "OrderedMeal_AddedProtein" INTEGER NOT NULL,
    "OrderedMeal_Calories" REAL NOT NULL,
    "OrderedMeal_Carbs" REAL NOT NULL,
    "OrderedMeal_DescriptionAR" TEXT NOT NULL,
    "OrderedMeal_DescriptionEN" TEXT NOT NULL,
    "OrderedMeal_Fats" REAL NOT NULL,
    "OrderedMeal_Fibers" REAL NOT NULL,
    "OrderedMeal_MealId" INTEGER NOT NULL,
    "OrderedMeal_NameAR" TEXT NOT NULL,
    "OrderedMeal_NameEN" TEXT NOT NULL,
    "OrderedMeal_PictureUrl" TEXT NOT NULL,
    "OrderedMeal_PricePerCarb" REAL NOT NULL,
    "OrderedMeal_PricePerProtein" REAL NOT NULL,
    "OrderedMeal_Protein" REAL NOT NULL,
    "PointsPrice" INTEGER NOT NULL,
    "Price" decimal(18,2) NOT NULL,
    "Quantity" INTEGER NOT NULL,
    CONSTRAINT "FK_OrderItems_Orders_OrderId" FOREIGN KEY ("OrderId") REFERENCES "Orders" ("Id") ON DELETE CASCADE
);

INSERT INTO "ef_temp_OrderItems" ("Id", "GainedPoint", "OrderId", "OrderedMeal_AddedCarb", "OrderedMeal_AddedProtein", "OrderedMeal_Calories", "OrderedMeal_Carbs", "OrderedMeal_DescriptionAR", "OrderedMeal_DescriptionEN", "OrderedMeal_Fats", "OrderedMeal_Fibers", "OrderedMeal_MealId", "OrderedMeal_NameAR", "OrderedMeal_NameEN", "OrderedMeal_PictureUrl", "OrderedMeal_PricePerCarb", "OrderedMeal_PricePerProtein", "OrderedMeal_Protein", "PointsPrice", "Price", "Quantity")
SELECT "Id", "GainedPoint", "OrderId", "OrderedMeal_AddedCarb", "OrderedMeal_AddedProtein", "OrderedMeal_Calories", "OrderedMeal_Carbs", "OrderedMeal_DescriptionAR", "OrderedMeal_DescriptionEN", "OrderedMeal_Fats", "OrderedMeal_Fibers", "OrderedMeal_MealId", "OrderedMeal_NameAR", "OrderedMeal_NameEN", "OrderedMeal_PictureUrl", "OrderedMeal_PricePerCarb", "OrderedMeal_PricePerProtein", "OrderedMeal_Protein", "PointsPrice", "Price", "Quantity"
FROM "OrderItems";

COMMIT;

PRAGMA foreign_keys = 0;

BEGIN TRANSACTION;

DROP TABLE "Questions";

ALTER TABLE "ef_temp_Questions" RENAME TO "Questions";

DROP TABLE "OrderItems";

ALTER TABLE "ef_temp_OrderItems" RENAME TO "OrderItems";

COMMIT;

PRAGMA foreign_keys = 1;

BEGIN TRANSACTION;

CREATE INDEX "IX_Questions_ParentFAQId" ON "Questions" ("ParentFAQId");

CREATE INDEX "IX_Questions_ParentFAQId1" ON "Questions" ("ParentFAQId1");

CREATE INDEX "IX_OrderItems_OrderId" ON "OrderItems" ("OrderId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240228035519_f', '7.0.4');

COMMIT;

BEGIN TRANSACTION;

DROP INDEX "IX_Questions_ParentFAQId1";

CREATE TABLE "ef_temp_Questions" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Questions" PRIMARY KEY AUTOINCREMENT,
    "ParentFAQId" INTEGER NULL,
    "Title" TEXT NOT NULL,
    CONSTRAINT "FK_Questions_Questions_ParentFAQId" FOREIGN KEY ("ParentFAQId") REFERENCES "Questions" ("Id") ON DELETE CASCADE
);

INSERT INTO "ef_temp_Questions" ("Id", "ParentFAQId", "Title")
SELECT "Id", "ParentFAQId", "Title"
FROM "Questions";

CREATE TABLE "ef_temp_OrderItems" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_OrderItems" PRIMARY KEY AUTOINCREMENT,
    "GainedPoint" INTEGER NOT NULL,
    "OrderId" INTEGER NULL,
    "OrderedMeal_AddedCarb" INTEGER NOT NULL,
    "OrderedMeal_AddedProtein" INTEGER NOT NULL,
    "OrderedMeal_Calories" REAL NOT NULL,
    "OrderedMeal_Carbs" REAL NOT NULL,
    "OrderedMeal_DescriptionAR" TEXT NOT NULL,
    "OrderedMeal_DescriptionEN" TEXT NOT NULL,
    "OrderedMeal_Fats" REAL NOT NULL,
    "OrderedMeal_Fibers" REAL NOT NULL,
    "OrderedMeal_MealId" INTEGER NOT NULL,
    "OrderedMeal_NameAR" TEXT NOT NULL,
    "OrderedMeal_NameEN" TEXT NOT NULL,
    "OrderedMeal_PictureUrl" TEXT NOT NULL,
    "OrderedMeal_PricePerCarb" REAL NOT NULL,
    "OrderedMeal_PricePerProtein" REAL NOT NULL,
    "OrderedMeal_Protein" REAL NOT NULL,
    "PointsPrice" INTEGER NOT NULL,
    "Price" decimal(18,2) NOT NULL,
    "Quantity" INTEGER NOT NULL,
    CONSTRAINT "FK_OrderItems_Orders_OrderId" FOREIGN KEY ("OrderId") REFERENCES "Orders" ("Id") ON DELETE CASCADE
);

INSERT INTO "ef_temp_OrderItems" ("Id", "GainedPoint", "OrderId", "OrderedMeal_AddedCarb", "OrderedMeal_AddedProtein", "OrderedMeal_Calories", "OrderedMeal_Carbs", "OrderedMeal_DescriptionAR", "OrderedMeal_DescriptionEN", "OrderedMeal_Fats", "OrderedMeal_Fibers", "OrderedMeal_MealId", "OrderedMeal_NameAR", "OrderedMeal_NameEN", "OrderedMeal_PictureUrl", "OrderedMeal_PricePerCarb", "OrderedMeal_PricePerProtein", "OrderedMeal_Protein", "PointsPrice", "Price", "Quantity")
SELECT "Id", "GainedPoint", "OrderId", "OrderedMeal_AddedCarb", "OrderedMeal_AddedProtein", "OrderedMeal_Calories", "OrderedMeal_Carbs", "OrderedMeal_DescriptionAR", "OrderedMeal_DescriptionEN", "OrderedMeal_Fats", "OrderedMeal_Fibers", "OrderedMeal_MealId", "OrderedMeal_NameAR", "OrderedMeal_NameEN", "OrderedMeal_PictureUrl", "OrderedMeal_PricePerCarb", "OrderedMeal_PricePerProtein", "OrderedMeal_Protein", "PointsPrice", "Price", "Quantity"
FROM "OrderItems";

COMMIT;

PRAGMA foreign_keys = 0;

BEGIN TRANSACTION;

DROP TABLE "Questions";

ALTER TABLE "ef_temp_Questions" RENAME TO "Questions";

DROP TABLE "OrderItems";

ALTER TABLE "ef_temp_OrderItems" RENAME TO "OrderItems";

COMMIT;

PRAGMA foreign_keys = 1;

BEGIN TRANSACTION;

CREATE INDEX "IX_Questions_ParentFAQId" ON "Questions" ("ParentFAQId");

CREATE INDEX "IX_OrderItems_OrderId" ON "OrderItems" ("OrderId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240228035917_cdcd', '7.0.4');

COMMIT;

BEGIN TRANSACTION;

CREATE TABLE "ef_temp_OrderItems" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_OrderItems" PRIMARY KEY AUTOINCREMENT,
    "GainedPoint" INTEGER NOT NULL,
    "OrderId" INTEGER NULL,
    "OrderedMeal_AddedCarb" INTEGER NOT NULL,
    "OrderedMeal_AddedProtein" INTEGER NOT NULL,
    "OrderedMeal_Calories" REAL NOT NULL,
    "OrderedMeal_Carbs" REAL NOT NULL,
    "OrderedMeal_DescriptionAR" TEXT NOT NULL,
    "OrderedMeal_DescriptionEN" TEXT NOT NULL,
    "OrderedMeal_Fats" REAL NOT NULL,
    "OrderedMeal_Fibers" REAL NOT NULL,
    "OrderedMeal_MealId" INTEGER NOT NULL,
    "OrderedMeal_NameAR" TEXT NOT NULL,
    "OrderedMeal_NameEN" TEXT NOT NULL,
    "OrderedMeal_PictureUrl" TEXT NOT NULL,
    "OrderedMeal_PricePerCarb" REAL NOT NULL,
    "OrderedMeal_PricePerProtein" REAL NOT NULL,
    "OrderedMeal_Protein" REAL NOT NULL,
    "PointsPrice" INTEGER NOT NULL,
    "Price" decimal(18,2) NOT NULL,
    "Quantity" INTEGER NOT NULL,
    CONSTRAINT "FK_OrderItems_Orders_OrderId" FOREIGN KEY ("OrderId") REFERENCES "Orders" ("Id") ON DELETE CASCADE
);

INSERT INTO "ef_temp_OrderItems" ("Id", "GainedPoint", "OrderId", "OrderedMeal_AddedCarb", "OrderedMeal_AddedProtein", "OrderedMeal_Calories", "OrderedMeal_Carbs", "OrderedMeal_DescriptionAR", "OrderedMeal_DescriptionEN", "OrderedMeal_Fats", "OrderedMeal_Fibers", "OrderedMeal_MealId", "OrderedMeal_NameAR", "OrderedMeal_NameEN", "OrderedMeal_PictureUrl", "OrderedMeal_PricePerCarb", "OrderedMeal_PricePerProtein", "OrderedMeal_Protein", "PointsPrice", "Price", "Quantity")
SELECT "Id", "GainedPoint", "OrderId", "OrderedMeal_AddedCarb", "OrderedMeal_AddedProtein", "OrderedMeal_Calories", "OrderedMeal_Carbs", "OrderedMeal_DescriptionAR", "OrderedMeal_DescriptionEN", "OrderedMeal_Fats", "OrderedMeal_Fibers", "OrderedMeal_MealId", "OrderedMeal_NameAR", "OrderedMeal_NameEN", "OrderedMeal_PictureUrl", "OrderedMeal_PricePerCarb", "OrderedMeal_PricePerProtein", "OrderedMeal_Protein", "PointsPrice", "Price", "Quantity"
FROM "OrderItems";

COMMIT;

PRAGMA foreign_keys = 0;

BEGIN TRANSACTION;

DROP TABLE "OrderItems";

ALTER TABLE "ef_temp_OrderItems" RENAME TO "OrderItems";

COMMIT;

PRAGMA foreign_keys = 1;

BEGIN TRANSACTION;

CREATE INDEX "IX_OrderItems_OrderId" ON "OrderItems" ("OrderId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240228040509_CC', '7.0.4');

COMMIT;

BEGIN TRANSACTION;

CREATE TABLE "ef_temp_Questions" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Questions" PRIMARY KEY AUTOINCREMENT,
    "ParentFAQId" INTEGER NULL,
    "Title" TEXT NOT NULL,
    CONSTRAINT "FK_Questions_Questions_ParentFAQId" FOREIGN KEY ("ParentFAQId") REFERENCES "Questions" ("Id")
);

INSERT INTO "ef_temp_Questions" ("Id", "ParentFAQId", "Title")
SELECT "Id", "ParentFAQId", "Title"
FROM "Questions";

CREATE TABLE "ef_temp_OrderItems" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_OrderItems" PRIMARY KEY AUTOINCREMENT,
    "GainedPoint" INTEGER NOT NULL,
    "OrderId" INTEGER NULL,
    "OrderedMeal_AddedCarb" INTEGER NOT NULL,
    "OrderedMeal_AddedProtein" INTEGER NOT NULL,
    "OrderedMeal_Calories" REAL NOT NULL,
    "OrderedMeal_Carbs" REAL NOT NULL,
    "OrderedMeal_DescriptionAR" TEXT NOT NULL,
    "OrderedMeal_DescriptionEN" TEXT NOT NULL,
    "OrderedMeal_Fats" REAL NOT NULL,
    "OrderedMeal_Fibers" REAL NOT NULL,
    "OrderedMeal_MealId" INTEGER NOT NULL,
    "OrderedMeal_NameAR" TEXT NOT NULL,
    "OrderedMeal_NameEN" TEXT NOT NULL,
    "OrderedMeal_PictureUrl" TEXT NOT NULL,
    "OrderedMeal_PricePerCarb" REAL NOT NULL,
    "OrderedMeal_PricePerProtein" REAL NOT NULL,
    "OrderedMeal_Protein" REAL NOT NULL,
    "PointsPrice" INTEGER NOT NULL,
    "Price" decimal(18,2) NOT NULL,
    "Quantity" INTEGER NOT NULL,
    CONSTRAINT "FK_OrderItems_Orders_OrderId" FOREIGN KEY ("OrderId") REFERENCES "Orders" ("Id") ON DELETE CASCADE
);

INSERT INTO "ef_temp_OrderItems" ("Id", "GainedPoint", "OrderId", "OrderedMeal_AddedCarb", "OrderedMeal_AddedProtein", "OrderedMeal_Calories", "OrderedMeal_Carbs", "OrderedMeal_DescriptionAR", "OrderedMeal_DescriptionEN", "OrderedMeal_Fats", "OrderedMeal_Fibers", "OrderedMeal_MealId", "OrderedMeal_NameAR", "OrderedMeal_NameEN", "OrderedMeal_PictureUrl", "OrderedMeal_PricePerCarb", "OrderedMeal_PricePerProtein", "OrderedMeal_Protein", "PointsPrice", "Price", "Quantity")
SELECT "Id", "GainedPoint", "OrderId", "OrderedMeal_AddedCarb", "OrderedMeal_AddedProtein", "OrderedMeal_Calories", "OrderedMeal_Carbs", "OrderedMeal_DescriptionAR", "OrderedMeal_DescriptionEN", "OrderedMeal_Fats", "OrderedMeal_Fibers", "OrderedMeal_MealId", "OrderedMeal_NameAR", "OrderedMeal_NameEN", "OrderedMeal_PictureUrl", "OrderedMeal_PricePerCarb", "OrderedMeal_PricePerProtein", "OrderedMeal_Protein", "PointsPrice", "Price", "Quantity"
FROM "OrderItems";

COMMIT;

PRAGMA foreign_keys = 0;

BEGIN TRANSACTION;

DROP TABLE "Questions";

ALTER TABLE "ef_temp_Questions" RENAME TO "Questions";

DROP TABLE "OrderItems";

ALTER TABLE "ef_temp_OrderItems" RENAME TO "OrderItems";

COMMIT;

PRAGMA foreign_keys = 1;

BEGIN TRANSACTION;

CREATE INDEX "IX_Questions_ParentFAQId" ON "Questions" ("ParentFAQId");

CREATE INDEX "IX_OrderItems_OrderId" ON "OrderItems" ("OrderId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240228040935_cdv', '7.0.4');

COMMIT;

BEGIN TRANSACTION;

ALTER TABLE "UserSelectedExtraOptions" ADD "Carb" REAL NOT NULL DEFAULT 0.0;

ALTER TABLE "UserSelectedExtraOptions" ADD "Fat" REAL NOT NULL DEFAULT 0.0;

ALTER TABLE "UserSelectedExtraOptions" ADD "Fiber" REAL NOT NULL DEFAULT 0.0;

ALTER TABLE "UserSelectedExtraOptions" ADD "Protein" REAL NOT NULL DEFAULT 0.0;

ALTER TABLE "UserSelectedDrinks" ADD "Carb" REAL NOT NULL DEFAULT 0.0;

ALTER TABLE "UserSelectedDrinks" ADD "Fat" REAL NOT NULL DEFAULT 0.0;

ALTER TABLE "UserSelectedDrinks" ADD "Fiber" REAL NOT NULL DEFAULT 0.0;

ALTER TABLE "UserSelectedDrinks" ADD "Protein" REAL NOT NULL DEFAULT 0.0;

ALTER TABLE "ExtraOptions" ADD "Carb" REAL NOT NULL DEFAULT 0.0;

ALTER TABLE "ExtraOptions" ADD "Fat" REAL NOT NULL DEFAULT 0.0;

ALTER TABLE "ExtraOptions" ADD "Fiber" REAL NOT NULL DEFAULT 0.0;

ALTER TABLE "ExtraOptions" ADD "Protein" REAL NOT NULL DEFAULT 0.0;

ALTER TABLE "Drinks" ADD "Carb" REAL NOT NULL DEFAULT 0.0;

ALTER TABLE "Drinks" ADD "Fat" REAL NOT NULL DEFAULT 0.0;

ALTER TABLE "Drinks" ADD "Fiber" REAL NOT NULL DEFAULT 0.0;

ALTER TABLE "Drinks" ADD "Protein" REAL NOT NULL DEFAULT 0.0;

CREATE TABLE "Cars" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Cars" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "IsAvailable" INTEGER NOT NULL,
    "WorkingStartHour" TEXT NOT NULL,
    "WorkingEndHour" TEXT NOT NULL
);

CREATE TABLE "Booking" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Booking" PRIMARY KEY AUTOINCREMENT,
    "CarId" INTEGER NOT NULL,
    "UserId" INTEGER NOT NULL,
    "StartTime" TEXT NOT NULL,
    "EndTime" TEXT NOT NULL,
    CONSTRAINT "FK_Booking_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Booking_Cars_CarId" FOREIGN KEY ("CarId") REFERENCES "Cars" ("Id") ON DELETE CASCADE
);

CREATE TABLE "CarWorkingDay" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_CarWorkingDay" PRIMARY KEY AUTOINCREMENT,
    "Day" INTEGER NOT NULL,
    "CarId" INTEGER NOT NULL,
    CONSTRAINT "FK_CarWorkingDay_Cars_CarId" FOREIGN KEY ("CarId") REFERENCES "Cars" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Booking_CarId" ON "Booking" ("CarId");

CREATE INDEX "IX_Booking_UserId" ON "Booking" ("UserId");

CREATE INDEX "IX_CarWorkingDay_CarId" ON "CarWorkingDay" ("CarId");

CREATE TABLE "ef_temp_UserSelectedSnacks" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_UserSelectedSnacks" PRIMARY KEY AUTOINCREMENT,
    "Carbs" REAL NOT NULL,
    "DescriptionAR" TEXT NOT NULL,
    "DescriptionEN" TEXT NOT NULL,
    "Fats" REAL NOT NULL,
    "Fibers" REAL NOT NULL,
    "NameAR" TEXT NOT NULL,
    "NameEN" TEXT NOT NULL,
    "PictureUrl" TEXT NOT NULL,
    "Protein" REAL NOT NULL,
    "Quantity" INTEGER NOT NULL,
    "UserPlanDayId" INTEGER NULL,
    CONSTRAINT "FK_UserSelectedSnacks_UserPlanDays_UserPlanDayId" FOREIGN KEY ("UserPlanDayId") REFERENCES "UserPlanDays" ("Id")
);

INSERT INTO "ef_temp_UserSelectedSnacks" ("Id", "Carbs", "DescriptionAR", "DescriptionEN", "Fats", "Fibers", "NameAR", "NameEN", "PictureUrl", "Protein", "Quantity", "UserPlanDayId")
SELECT "Id", "Carbs", "DescriptionAR", "DescriptionEN", "Fats", "Fibers", "NameAR", "NameEN", "PictureUrl", "Protein", "Quantity", "UserPlanDayId"
FROM "UserSelectedSnacks";

CREATE TABLE "ef_temp_OrderItems" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_OrderItems" PRIMARY KEY AUTOINCREMENT,
    "GainedPoint" INTEGER NOT NULL,
    "OrderId" INTEGER NULL,
    "OrderedMeal_AddedCarb" INTEGER NOT NULL,
    "OrderedMeal_AddedProtein" INTEGER NOT NULL,
    "OrderedMeal_Calories" REAL NOT NULL,
    "OrderedMeal_Carbs" REAL NOT NULL,
    "OrderedMeal_DescriptionAR" TEXT NOT NULL,
    "OrderedMeal_DescriptionEN" TEXT NOT NULL,
    "OrderedMeal_Fats" REAL NOT NULL,
    "OrderedMeal_Fibers" REAL NOT NULL,
    "OrderedMeal_MealId" INTEGER NOT NULL,
    "OrderedMeal_NameAR" TEXT NOT NULL,
    "OrderedMeal_NameEN" TEXT NOT NULL,
    "OrderedMeal_PictureUrl" TEXT NOT NULL,
    "OrderedMeal_PricePerCarb" REAL NOT NULL,
    "OrderedMeal_PricePerProtein" REAL NOT NULL,
    "OrderedMeal_Protein" REAL NOT NULL,
    "PointsPrice" INTEGER NOT NULL,
    "Price" decimal(18,2) NOT NULL,
    "Quantity" INTEGER NOT NULL,
    CONSTRAINT "FK_OrderItems_Orders_OrderId" FOREIGN KEY ("OrderId") REFERENCES "Orders" ("Id") ON DELETE CASCADE
);

INSERT INTO "ef_temp_OrderItems" ("Id", "GainedPoint", "OrderId", "OrderedMeal_AddedCarb", "OrderedMeal_AddedProtein", "OrderedMeal_Calories", "OrderedMeal_Carbs", "OrderedMeal_DescriptionAR", "OrderedMeal_DescriptionEN", "OrderedMeal_Fats", "OrderedMeal_Fibers", "OrderedMeal_MealId", "OrderedMeal_NameAR", "OrderedMeal_NameEN", "OrderedMeal_PictureUrl", "OrderedMeal_PricePerCarb", "OrderedMeal_PricePerProtein", "OrderedMeal_Protein", "PointsPrice", "Price", "Quantity")
SELECT "Id", "GainedPoint", "OrderId", "OrderedMeal_AddedCarb", "OrderedMeal_AddedProtein", "OrderedMeal_Calories", "OrderedMeal_Carbs", "OrderedMeal_DescriptionAR", "OrderedMeal_DescriptionEN", "OrderedMeal_Fats", "OrderedMeal_Fibers", "OrderedMeal_MealId", "OrderedMeal_NameAR", "OrderedMeal_NameEN", "OrderedMeal_PictureUrl", "OrderedMeal_PricePerCarb", "OrderedMeal_PricePerProtein", "OrderedMeal_Protein", "PointsPrice", "Price", "Quantity"
FROM "OrderItems";

COMMIT;

PRAGMA foreign_keys = 0;

BEGIN TRANSACTION;

DROP TABLE "UserSelectedSnacks";

ALTER TABLE "ef_temp_UserSelectedSnacks" RENAME TO "UserSelectedSnacks";

DROP TABLE "OrderItems";

ALTER TABLE "ef_temp_OrderItems" RENAME TO "OrderItems";

COMMIT;

PRAGMA foreign_keys = 1;

BEGIN TRANSACTION;

CREATE INDEX "IX_UserSelectedSnacks_UserPlanDayId" ON "UserSelectedSnacks" ("UserPlanDayId");

CREATE INDEX "IX_OrderItems_OrderId" ON "OrderItems" ("OrderId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240228051025_cars', '7.0.4');

COMMIT;

BEGIN TRANSACTION;

CREATE TABLE "ef_temp_OrderItems" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_OrderItems" PRIMARY KEY AUTOINCREMENT,
    "GainedPoint" INTEGER NOT NULL,
    "OrderId" INTEGER NULL,
    "OrderedMeal_AddedCarb" INTEGER NOT NULL,
    "OrderedMeal_AddedProtein" INTEGER NOT NULL,
    "OrderedMeal_Calories" REAL NOT NULL,
    "OrderedMeal_Carbs" REAL NOT NULL,
    "OrderedMeal_DescriptionAR" TEXT NOT NULL,
    "OrderedMeal_DescriptionEN" TEXT NOT NULL,
    "OrderedMeal_Fats" REAL NOT NULL,
    "OrderedMeal_Fibers" REAL NOT NULL,
    "OrderedMeal_MealId" INTEGER NOT NULL,
    "OrderedMeal_NameAR" TEXT NOT NULL,
    "OrderedMeal_NameEN" TEXT NOT NULL,
    "OrderedMeal_PictureUrl" TEXT NOT NULL,
    "OrderedMeal_PricePerCarb" REAL NOT NULL,
    "OrderedMeal_PricePerProtein" REAL NOT NULL,
    "OrderedMeal_Protein" REAL NOT NULL,
    "PointsPrice" INTEGER NOT NULL,
    "Price" decimal(18,2) NOT NULL,
    "Quantity" INTEGER NOT NULL,
    CONSTRAINT "FK_OrderItems_Orders_OrderId" FOREIGN KEY ("OrderId") REFERENCES "Orders" ("Id") ON DELETE CASCADE
);

INSERT INTO "ef_temp_OrderItems" ("Id", "GainedPoint", "OrderId", "OrderedMeal_AddedCarb", "OrderedMeal_AddedProtein", "OrderedMeal_Calories", "OrderedMeal_Carbs", "OrderedMeal_DescriptionAR", "OrderedMeal_DescriptionEN", "OrderedMeal_Fats", "OrderedMeal_Fibers", "OrderedMeal_MealId", "OrderedMeal_NameAR", "OrderedMeal_NameEN", "OrderedMeal_PictureUrl", "OrderedMeal_PricePerCarb", "OrderedMeal_PricePerProtein", "OrderedMeal_Protein", "PointsPrice", "Price", "Quantity")
SELECT "Id", "GainedPoint", "OrderId", "OrderedMeal_AddedCarb", "OrderedMeal_AddedProtein", "OrderedMeal_Calories", "OrderedMeal_Carbs", "OrderedMeal_DescriptionAR", "OrderedMeal_DescriptionEN", "OrderedMeal_Fats", "OrderedMeal_Fibers", "OrderedMeal_MealId", "OrderedMeal_NameAR", "OrderedMeal_NameEN", "OrderedMeal_PictureUrl", "OrderedMeal_PricePerCarb", "OrderedMeal_PricePerProtein", "OrderedMeal_Protein", "PointsPrice", "Price", "Quantity"
FROM "OrderItems";

CREATE TABLE "ef_temp_Cars" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Cars" PRIMARY KEY AUTOINCREMENT,
    "IsAvailable" INTEGER NOT NULL,
    "Name" TEXT NOT NULL,
    "WorkingEndHour" INTEGER NOT NULL,
    "WorkingStartHour" INTEGER NOT NULL
);

INSERT INTO "ef_temp_Cars" ("Id", "IsAvailable", "Name", "WorkingEndHour", "WorkingStartHour")
SELECT "Id", "IsAvailable", "Name", "WorkingEndHour", "WorkingStartHour"
FROM "Cars";

COMMIT;

PRAGMA foreign_keys = 0;

BEGIN TRANSACTION;

DROP TABLE "OrderItems";

ALTER TABLE "ef_temp_OrderItems" RENAME TO "OrderItems";

DROP TABLE "Cars";

ALTER TABLE "ef_temp_Cars" RENAME TO "Cars";

COMMIT;

PRAGMA foreign_keys = 1;

BEGIN TRANSACTION;

CREATE INDEX "IX_OrderItems_OrderId" ON "OrderItems" ("OrderId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240229042310_vvvvv', '7.0.4');

COMMIT;

BEGIN TRANSACTION;

CREATE TABLE "Bundles" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Bundles" PRIMARY KEY AUTOINCREMENT,
    "Price" REAL NOT NULL,
    "Duration" INTEGER NOT NULL,
    "MealPerDay" INTEGER NOT NULL
);

CREATE TABLE "ef_temp_OrderItems" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_OrderItems" PRIMARY KEY AUTOINCREMENT,
    "GainedPoint" INTEGER NOT NULL,
    "OrderId" INTEGER NULL,
    "OrderedMeal_AddedCarb" INTEGER NOT NULL,
    "OrderedMeal_AddedProtein" INTEGER NOT NULL,
    "OrderedMeal_Calories" REAL NOT NULL,
    "OrderedMeal_Carbs" REAL NOT NULL,
    "OrderedMeal_DescriptionAR" TEXT NOT NULL,
    "OrderedMeal_DescriptionEN" TEXT NOT NULL,
    "OrderedMeal_Fats" REAL NOT NULL,
    "OrderedMeal_Fibers" REAL NOT NULL,
    "OrderedMeal_MealId" INTEGER NOT NULL,
    "OrderedMeal_NameAR" TEXT NOT NULL,
    "OrderedMeal_NameEN" TEXT NOT NULL,
    "OrderedMeal_PictureUrl" TEXT NOT NULL,
    "OrderedMeal_PricePerCarb" REAL NOT NULL,
    "OrderedMeal_PricePerProtein" REAL NOT NULL,
    "OrderedMeal_Protein" REAL NOT NULL,
    "PointsPrice" INTEGER NOT NULL,
    "Price" decimal(18,2) NOT NULL,
    "Quantity" INTEGER NOT NULL,
    CONSTRAINT "FK_OrderItems_Orders_OrderId" FOREIGN KEY ("OrderId") REFERENCES "Orders" ("Id") ON DELETE CASCADE
);

INSERT INTO "ef_temp_OrderItems" ("Id", "GainedPoint", "OrderId", "OrderedMeal_AddedCarb", "OrderedMeal_AddedProtein", "OrderedMeal_Calories", "OrderedMeal_Carbs", "OrderedMeal_DescriptionAR", "OrderedMeal_DescriptionEN", "OrderedMeal_Fats", "OrderedMeal_Fibers", "OrderedMeal_MealId", "OrderedMeal_NameAR", "OrderedMeal_NameEN", "OrderedMeal_PictureUrl", "OrderedMeal_PricePerCarb", "OrderedMeal_PricePerProtein", "OrderedMeal_Protein", "PointsPrice", "Price", "Quantity")
SELECT "Id", "GainedPoint", "OrderId", "OrderedMeal_AddedCarb", "OrderedMeal_AddedProtein", "OrderedMeal_Calories", "OrderedMeal_Carbs", "OrderedMeal_DescriptionAR", "OrderedMeal_DescriptionEN", "OrderedMeal_Fats", "OrderedMeal_Fibers", "OrderedMeal_MealId", "OrderedMeal_NameAR", "OrderedMeal_NameEN", "OrderedMeal_PictureUrl", "OrderedMeal_PricePerCarb", "OrderedMeal_PricePerProtein", "OrderedMeal_Protein", "PointsPrice", "Price", "Quantity"
FROM "OrderItems";

CREATE TABLE "ef_temp_Cars" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Cars" PRIMARY KEY AUTOINCREMENT,
    "IsAvailable" INTEGER NOT NULL,
    "Name" TEXT NOT NULL,
    "WorkingEndHour" TEXT NOT NULL,
    "WorkingStartHour" TEXT NOT NULL
);

INSERT INTO "ef_temp_Cars" ("Id", "IsAvailable", "Name", "WorkingEndHour", "WorkingStartHour")
SELECT "Id", "IsAvailable", "Name", "WorkingEndHour", "WorkingStartHour"
FROM "Cars";

COMMIT;

PRAGMA foreign_keys = 0;

BEGIN TRANSACTION;

DROP TABLE "OrderItems";

ALTER TABLE "ef_temp_OrderItems" RENAME TO "OrderItems";

DROP TABLE "Cars";

ALTER TABLE "ef_temp_Cars" RENAME TO "Cars";

COMMIT;

PRAGMA foreign_keys = 1;

BEGIN TRANSACTION;

CREATE INDEX "IX_OrderItems_OrderId" ON "OrderItems" ("OrderId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240229100809_bundle', '7.0.4');

COMMIT;

BEGIN TRANSACTION;

ALTER TABLE "Bundles" RENAME COLUMN "MealPerDay" TO "MealsPerDay";

CREATE TABLE "ef_temp_OrderItems" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_OrderItems" PRIMARY KEY AUTOINCREMENT,
    "GainedPoint" INTEGER NOT NULL,
    "OrderId" INTEGER NULL,
    "OrderedMeal_AddedCarb" INTEGER NOT NULL,
    "OrderedMeal_AddedProtein" INTEGER NOT NULL,
    "OrderedMeal_Calories" REAL NOT NULL,
    "OrderedMeal_Carbs" REAL NOT NULL,
    "OrderedMeal_DescriptionAR" TEXT NOT NULL,
    "OrderedMeal_DescriptionEN" TEXT NOT NULL,
    "OrderedMeal_Fats" REAL NOT NULL,
    "OrderedMeal_Fibers" REAL NOT NULL,
    "OrderedMeal_MealId" INTEGER NOT NULL,
    "OrderedMeal_NameAR" TEXT NOT NULL,
    "OrderedMeal_NameEN" TEXT NOT NULL,
    "OrderedMeal_PictureUrl" TEXT NOT NULL,
    "OrderedMeal_PricePerCarb" REAL NOT NULL,
    "OrderedMeal_PricePerProtein" REAL NOT NULL,
    "OrderedMeal_Protein" REAL NOT NULL,
    "PointsPrice" INTEGER NOT NULL,
    "Price" decimal(18,2) NOT NULL,
    "Quantity" INTEGER NOT NULL,
    CONSTRAINT "FK_OrderItems_Orders_OrderId" FOREIGN KEY ("OrderId") REFERENCES "Orders" ("Id") ON DELETE CASCADE
);

INSERT INTO "ef_temp_OrderItems" ("Id", "GainedPoint", "OrderId", "OrderedMeal_AddedCarb", "OrderedMeal_AddedProtein", "OrderedMeal_Calories", "OrderedMeal_Carbs", "OrderedMeal_DescriptionAR", "OrderedMeal_DescriptionEN", "OrderedMeal_Fats", "OrderedMeal_Fibers", "OrderedMeal_MealId", "OrderedMeal_NameAR", "OrderedMeal_NameEN", "OrderedMeal_PictureUrl", "OrderedMeal_PricePerCarb", "OrderedMeal_PricePerProtein", "OrderedMeal_Protein", "PointsPrice", "Price", "Quantity")
SELECT "Id", "GainedPoint", "OrderId", "OrderedMeal_AddedCarb", "OrderedMeal_AddedProtein", "OrderedMeal_Calories", "OrderedMeal_Carbs", "OrderedMeal_DescriptionAR", "OrderedMeal_DescriptionEN", "OrderedMeal_Fats", "OrderedMeal_Fibers", "OrderedMeal_MealId", "OrderedMeal_NameAR", "OrderedMeal_NameEN", "OrderedMeal_PictureUrl", "OrderedMeal_PricePerCarb", "OrderedMeal_PricePerProtein", "OrderedMeal_Protein", "PointsPrice", "Price", "Quantity"
FROM "OrderItems";

COMMIT;

PRAGMA foreign_keys = 0;

BEGIN TRANSACTION;

DROP TABLE "OrderItems";

ALTER TABLE "ef_temp_OrderItems" RENAME TO "OrderItems";

COMMIT;

PRAGMA foreign_keys = 1;

BEGIN TRANSACTION;

CREATE INDEX "IX_OrderItems_OrderId" ON "OrderItems" ("OrderId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240229101016_mm', '7.0.4');

COMMIT;

BEGIN TRANSACTION;

CREATE TABLE "ef_temp_OrderItems" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_OrderItems" PRIMARY KEY AUTOINCREMENT,
    "GainedPoint" INTEGER NOT NULL,
    "OrderId" INTEGER NULL,
    "OrderedMeal_AddedCarb" INTEGER NOT NULL,
    "OrderedMeal_AddedProtein" INTEGER NOT NULL,
    "OrderedMeal_Calories" REAL NOT NULL,
    "OrderedMeal_Carbs" REAL NOT NULL,
    "OrderedMeal_DescriptionAR" TEXT NOT NULL,
    "OrderedMeal_DescriptionEN" TEXT NOT NULL,
    "OrderedMeal_Fats" REAL NOT NULL,
    "OrderedMeal_Fibers" REAL NOT NULL,
    "OrderedMeal_MealId" INTEGER NOT NULL,
    "OrderedMeal_NameAR" TEXT NOT NULL,
    "OrderedMeal_NameEN" TEXT NOT NULL,
    "OrderedMeal_PictureUrl" TEXT NOT NULL,
    "OrderedMeal_PricePerCarb" REAL NOT NULL,
    "OrderedMeal_PricePerProtein" REAL NOT NULL,
    "OrderedMeal_Protein" REAL NOT NULL,
    "PointsPrice" INTEGER NOT NULL,
    "Price" decimal(18,2) NOT NULL,
    "Quantity" INTEGER NOT NULL,
    CONSTRAINT "FK_OrderItems_Orders_OrderId" FOREIGN KEY ("OrderId") REFERENCES "Orders" ("Id") ON DELETE CASCADE
);

INSERT INTO "ef_temp_OrderItems" ("Id", "GainedPoint", "OrderId", "OrderedMeal_AddedCarb", "OrderedMeal_AddedProtein", "OrderedMeal_Calories", "OrderedMeal_Carbs", "OrderedMeal_DescriptionAR", "OrderedMeal_DescriptionEN", "OrderedMeal_Fats", "OrderedMeal_Fibers", "OrderedMeal_MealId", "OrderedMeal_NameAR", "OrderedMeal_NameEN", "OrderedMeal_PictureUrl", "OrderedMeal_PricePerCarb", "OrderedMeal_PricePerProtein", "OrderedMeal_Protein", "PointsPrice", "Price", "Quantity")
SELECT "Id", "GainedPoint", "OrderId", "OrderedMeal_AddedCarb", "OrderedMeal_AddedProtein", "OrderedMeal_Calories", "OrderedMeal_Carbs", "OrderedMeal_DescriptionAR", "OrderedMeal_DescriptionEN", "OrderedMeal_Fats", "OrderedMeal_Fibers", "OrderedMeal_MealId", "OrderedMeal_NameAR", "OrderedMeal_NameEN", "OrderedMeal_PictureUrl", "OrderedMeal_PricePerCarb", "OrderedMeal_PricePerProtein", "OrderedMeal_Protein", "PointsPrice", "Price", "Quantity"
FROM "OrderItems";

COMMIT;

PRAGMA foreign_keys = 0;

BEGIN TRANSACTION;

DROP TABLE "OrderItems";

ALTER TABLE "ef_temp_OrderItems" RENAME TO "OrderItems";

COMMIT;

PRAGMA foreign_keys = 1;

BEGIN TRANSACTION;

CREATE INDEX "IX_OrderItems_OrderId" ON "OrderItems" ("OrderId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240229105503_p', '7.0.4');

COMMIT;

