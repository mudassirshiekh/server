﻿CREATE PROCEDURE [dbo].[Organization_Create]
    @Id UNIQUEIDENTIFIER OUTPUT,
    @Identifier NVARCHAR(50),
    @Name NVARCHAR(50),
    @BusinessName NVARCHAR(50),
    @BusinessAddress1 NVARCHAR(50),
    @BusinessAddress2 NVARCHAR(50),
    @BusinessAddress3 NVARCHAR(50),
    @BusinessCountry VARCHAR(2),
    @BusinessTaxNumber NVARCHAR(30),
    @BillingEmail NVARCHAR(256),
    @Plan NVARCHAR(50) = NULL,
    @PlanType TINYINT = NULL,
    @Seats INT = 0,
    @MaxCollections SMALLINT,
    @UsePolicies BIT,
    @UseSso BIT,
    @UseGroups BIT,
    @UseDirectory BIT,
    @UseEvents BIT,
    @UseTotp BIT = 0,
    @Use2fa BIT,
    @UseApi BIT,
    @UseResetPassword BIT,
    @SelfHost BIT,
    @UsersGetPremium BIT = 0,
    @Storage BIGINT = 0,
    @MaxStorageGb SMALLINT = 0,
    @Gateway TINYINT,
    @GatewayCustomerId VARCHAR(50),
    @GatewaySubscriptionId VARCHAR(50),
    @ReferenceData VARCHAR(MAX),
    @Enabled BIT,
    @LicenseKey VARCHAR(100),
    @PublicKey VARCHAR(MAX),
    @PrivateKey VARCHAR(MAX),
    @TwoFactorProviders NVARCHAR(MAX),
    @ExpirationDate DATETIME2(7),
    @CreationDate DATETIME2(7),
    @RevisionDate DATETIME2(7),
    @OwnersNotifiedOfAutoscaling DATETIME2(7),
    @MaxAutoscaleSeats INT = 0,
    @UseKeyConnector BIT = 0,
    @UseScim BIT = 0
AS
BEGIN
    SET NOCOUNT ON

    INSERT INTO [dbo].[Organization]
    (
        [Id],
        [Identifier],
        [Name],
        [BusinessName],
        [BusinessAddress1],
        [BusinessAddress2],
        [BusinessAddress3],
        [BusinessCountry],
        [BusinessTaxNumber],
        [BillingEmail],
        [Plan],
        [PlanType],
        [Seats],
        [MaxCollections],
        [UsePolicies],
        [UseSso],
        [UseGroups],
        [UseDirectory],
        [UseEvents],
        [UseTotp],
        [Use2fa],
        [UseApi],
        [UseResetPassword],
        [SelfHost],
        [UsersGetPremium],
        [Storage],
        [MaxStorageGb],
        [Gateway],
        [GatewayCustomerId],
        [GatewaySubscriptionId],
        [ReferenceData],
        [Enabled],
        [LicenseKey],
        [PublicKey],
        [PrivateKey],
        [TwoFactorProviders],
        [ExpirationDate],
        [CreationDate],
        [RevisionDate],
        [OwnersNotifiedOfAutoscaling],
        [MaxAutoscaleSeats],
        [UseKeyConnector],
        [UseScim]
    )
    VALUES
    (
        @Id,
        @Identifier,
        @Name,
        @BusinessName,
        @BusinessAddress1,
        @BusinessAddress2,
        @BusinessAddress3,
        @BusinessCountry,
        @BusinessTaxNumber,
        @BillingEmail,
        @Plan,
        @PlanType,
        @Seats,
        @MaxCollections,
        @UsePolicies,
        @UseSso,
        @UseGroups,
        @UseDirectory,
        @UseEvents,
        @UseTotp,
        @Use2fa,
        @UseApi,
        @UseResetPassword,
        @SelfHost,
        @UsersGetPremium,
        @Storage,
        @MaxStorageGb,
        @Gateway,
        @GatewayCustomerId,
        @GatewaySubscriptionId,
        @ReferenceData,
        @Enabled,
        @LicenseKey,
        @PublicKey,
        @PrivateKey,
        @TwoFactorProviders,
        @ExpirationDate,
        @CreationDate,
        @RevisionDate,
        @OwnersNotifiedOfAutoscaling,
        @MaxAutoscaleSeats,
        @UseKeyConnector,
        @UseScim
    )

    IF @Plan != null
    BEGIN
        INSERT INTO [dbo].[OrganizationPasswordManager]
        (
            [OrganizationId],
            [Plan],
            [PlanType],
            [Seats],
            [MaxCollections],
            [UseTotp],
            [UsersGetPremium],
            [Storage],
            [MaxStorageGb],
            [MaxAutoscaleSeats],
            [RevisionDate]
        )
        VALUES
        (
            @Id,
            @Plan,
            @PlanType,
            @Seats,
            @MaxCollections,
            @UseTotp,
            @UsersGetPremium,
            @Storage,
            @MaxStorageGb,
            @MaxAutoscaleSeats,
            GETUTCDATE()
        )
    END
END