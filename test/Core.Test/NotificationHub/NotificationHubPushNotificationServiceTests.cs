﻿#nullable enable
using System.Text.Json;
using Bit.Core.Enums;
using Bit.Core.Models;
using Bit.Core.Models.Data;
using Bit.Core.NotificationCenter.Entities;
using Bit.Core.NotificationHub;
using Bit.Core.Repositories;
using Bit.Core.Test.NotificationCenter.AutoFixture;
using Bit.Test.Common.AutoFixture;
using Bit.Test.Common.AutoFixture.Attributes;
using NSubstitute;
using Xunit;

namespace Bit.Core.Test.NotificationHub;

[SutProviderCustomize]
[NotificationStatusCustomize]
public class NotificationHubPushNotificationServiceTests
{
    [Theory]
    [BitAutoData]
    [NotificationCustomize]
    public async Task PushSyncNotificationAsync_Global_NotSent(
        SutProvider<NotificationHubPushNotificationService> sutProvider, Notification notification)
    {
        await sutProvider.Sut.PushSyncNotificationAsync(notification);

        await sutProvider.GetDependency<INotificationHubPool>()
            .Received(0)
            .AllClients
            .Received(0)
            .SendTemplateNotificationAsync(Arg.Any<IDictionary<string, string>>(), Arg.Any<string>());
        await sutProvider.GetDependency<IInstallationDeviceRepository>()
            .Received(0)
            .UpsertAsync(Arg.Any<InstallationDeviceEntity>());
    }

    [Theory]
    [BitAutoData(false)]
    [BitAutoData(true)]
    [NotificationCustomize(false)]
    public async Task PushSyncNotificationAsync_UserIdProvidedClientTypeAll_SentToUser(
        bool organizationIdNull, SutProvider<NotificationHubPushNotificationService> sutProvider,
        Notification notification)
    {
        if (organizationIdNull)
        {
            notification.OrganizationId = null;
        }

        notification.ClientType = ClientType.All;
        var expectedSyncNotification = ToSyncNotificationPushNotification(notification, null);

        await sutProvider.Sut.PushSyncNotificationAsync(notification);

        await AssertSendTemplateNotificationAsync(sutProvider, PushType.SyncNotification,
            expectedSyncNotification,
            $"(template:payload_userId:{notification.UserId})");
        await sutProvider.GetDependency<IInstallationDeviceRepository>()
            .Received(0)
            .UpsertAsync(Arg.Any<InstallationDeviceEntity>());
    }

    [Theory]
    [BitAutoData(ClientType.Browser)]
    [BitAutoData(ClientType.Desktop)]
    [BitAutoData(ClientType.Web)]
    [BitAutoData(ClientType.Mobile)]
    [NotificationCustomize(false)]
    public async Task PushSyncNotificationAsync_UserIdProvidedOrganizationIdNullClientTypeNotAll_SentToUser(
        ClientType clientType, SutProvider<NotificationHubPushNotificationService> sutProvider,
        Notification notification)
    {
        notification.OrganizationId = null;
        notification.ClientType = clientType;
        var expectedSyncNotification = ToSyncNotificationPushNotification(notification, null);

        await sutProvider.Sut.PushSyncNotificationAsync(notification);

        await AssertSendTemplateNotificationAsync(sutProvider, PushType.SyncNotification,
            expectedSyncNotification,
            $"(template:payload_userId:{notification.UserId} && clientType:{clientType})");
        await sutProvider.GetDependency<IInstallationDeviceRepository>()
            .Received(0)
            .UpsertAsync(Arg.Any<InstallationDeviceEntity>());
    }

    [Theory]
    [BitAutoData(ClientType.Browser)]
    [BitAutoData(ClientType.Desktop)]
    [BitAutoData(ClientType.Web)]
    [BitAutoData(ClientType.Mobile)]
    [NotificationCustomize(false)]
    public async Task PushSyncNotificationAsync_UserIdProvidedOrganizationIdProvidedClientTypeNotAll_SentToUser(
        ClientType clientType, SutProvider<NotificationHubPushNotificationService> sutProvider,
        Notification notification)
    {
        notification.ClientType = clientType;
        var expectedSyncNotification = ToSyncNotificationPushNotification(notification, null);

        await sutProvider.Sut.PushSyncNotificationAsync(notification);

        await AssertSendTemplateNotificationAsync(sutProvider, PushType.SyncNotification,
            expectedSyncNotification,
            $"(template:payload_userId:{notification.UserId} && clientType:{clientType})");
        await sutProvider.GetDependency<IInstallationDeviceRepository>()
            .Received(0)
            .UpsertAsync(Arg.Any<InstallationDeviceEntity>());
    }

    [Theory]
    [BitAutoData]
    [NotificationCustomize(false)]
    public async Task PushSyncNotificationAsync_UserIdNullOrganizationIdProvidedClientTypeAll_SentToOrganization(
        SutProvider<NotificationHubPushNotificationService> sutProvider, Notification notification)
    {
        notification.UserId = null;
        notification.ClientType = ClientType.All;
        var expectedSyncNotification = ToSyncNotificationPushNotification(notification, null);

        await sutProvider.Sut.PushSyncNotificationAsync(notification);

        await AssertSendTemplateNotificationAsync(sutProvider, PushType.SyncNotification,
            expectedSyncNotification,
            $"(template:payload && organizationId:{notification.OrganizationId})");
        await sutProvider.GetDependency<IInstallationDeviceRepository>()
            .Received(0)
            .UpsertAsync(Arg.Any<InstallationDeviceEntity>());
    }

    [Theory]
    [BitAutoData(ClientType.Browser)]
    [BitAutoData(ClientType.Desktop)]
    [BitAutoData(ClientType.Web)]
    [BitAutoData(ClientType.Mobile)]
    [NotificationCustomize(false)]
    public async Task PushSyncNotificationAsync_UserIdNullOrganizationIdProvidedClientTypeNotAll_SentToOrganization(
        ClientType clientType, SutProvider<NotificationHubPushNotificationService> sutProvider,
        Notification notification)
    {
        notification.UserId = null;
        notification.ClientType = clientType;
        var expectedSyncNotification = ToSyncNotificationPushNotification(notification, null);

        await sutProvider.Sut.PushSyncNotificationAsync(notification);

        await AssertSendTemplateNotificationAsync(sutProvider, PushType.SyncNotification,
            expectedSyncNotification,
            $"(template:payload && organizationId:{notification.OrganizationId} && clientType:{clientType})");
        await sutProvider.GetDependency<IInstallationDeviceRepository>()
            .Received(0)
            .UpsertAsync(Arg.Any<InstallationDeviceEntity>());
    }

    [Theory]
    [BitAutoData]
    [NotificationCustomize]
    public async Task PushSyncNotificationStatusAsync_Global_NotSent(
        SutProvider<NotificationHubPushNotificationService> sutProvider, Notification notification,
        NotificationStatus notificationStatus)
    {
        await sutProvider.Sut.PushSyncNotificationStatusAsync(notification, notificationStatus);

        await sutProvider.GetDependency<INotificationHubPool>()
            .Received(0)
            .AllClients
            .Received(0)
            .SendTemplateNotificationAsync(Arg.Any<IDictionary<string, string>>(), Arg.Any<string>());
        await sutProvider.GetDependency<IInstallationDeviceRepository>()
            .Received(0)
            .UpsertAsync(Arg.Any<InstallationDeviceEntity>());
    }

    [Theory]
    [BitAutoData(false)]
    [BitAutoData(true)]
    [NotificationCustomize(false)]
    public async Task PushSyncNotificationStatusAsync_UserIdProvidedClientTypeAll_SentToUser(
        bool organizationIdNull, SutProvider<NotificationHubPushNotificationService> sutProvider,
        Notification notification, NotificationStatus notificationStatus)
    {
        if (organizationIdNull)
        {
            notification.OrganizationId = null;
        }

        notification.ClientType = ClientType.All;
        var expectedSyncNotification = ToSyncNotificationPushNotification(notification, notificationStatus);

        await sutProvider.Sut.PushSyncNotificationStatusAsync(notification, notificationStatus);

        await AssertSendTemplateNotificationAsync(sutProvider, PushType.SyncNotificationStatus,
            expectedSyncNotification,
            $"(template:payload_userId:{notification.UserId})");
        await sutProvider.GetDependency<IInstallationDeviceRepository>()
            .Received(0)
            .UpsertAsync(Arg.Any<InstallationDeviceEntity>());
    }

    [Theory]
    [BitAutoData(ClientType.Browser)]
    [BitAutoData(ClientType.Desktop)]
    [BitAutoData(ClientType.Web)]
    [BitAutoData(ClientType.Mobile)]
    [NotificationCustomize(false)]
    public async Task PushSyncNotificationStatusAsync_UserIdProvidedOrganizationIdNullClientTypeNotAll_SentToUser(
        ClientType clientType, SutProvider<NotificationHubPushNotificationService> sutProvider,
        Notification notification, NotificationStatus notificationStatus)
    {
        notification.OrganizationId = null;
        notification.ClientType = clientType;
        var expectedSyncNotification = ToSyncNotificationPushNotification(notification, notificationStatus);

        await sutProvider.Sut.PushSyncNotificationStatusAsync(notification, notificationStatus);

        await AssertSendTemplateNotificationAsync(sutProvider, PushType.SyncNotificationStatus,
            expectedSyncNotification,
            $"(template:payload_userId:{notification.UserId} && clientType:{clientType})");
        await sutProvider.GetDependency<IInstallationDeviceRepository>()
            .Received(0)
            .UpsertAsync(Arg.Any<InstallationDeviceEntity>());
    }

    [Theory]
    [BitAutoData(ClientType.Browser)]
    [BitAutoData(ClientType.Desktop)]
    [BitAutoData(ClientType.Web)]
    [BitAutoData(ClientType.Mobile)]
    [NotificationCustomize(false)]
    public async Task PushSyncNotificationStatusAsync_UserIdProvidedOrganizationIdProvidedClientTypeNotAll_SentToUser(
        ClientType clientType, SutProvider<NotificationHubPushNotificationService> sutProvider,
        Notification notification, NotificationStatus notificationStatus)
    {
        notification.ClientType = clientType;
        var expectedSyncNotification = ToSyncNotificationPushNotification(notification, notificationStatus);

        await sutProvider.Sut.PushSyncNotificationStatusAsync(notification, notificationStatus);

        await AssertSendTemplateNotificationAsync(sutProvider, PushType.SyncNotificationStatus,
            expectedSyncNotification,
            $"(template:payload_userId:{notification.UserId} && clientType:{clientType})");
        await sutProvider.GetDependency<IInstallationDeviceRepository>()
            .Received(0)
            .UpsertAsync(Arg.Any<InstallationDeviceEntity>());
    }

    [Theory]
    [BitAutoData]
    [NotificationCustomize(false)]
    public async Task PushSyncNotificationStatusAsync_UserIdNullOrganizationIdProvidedClientTypeAll_SentToOrganization(
        SutProvider<NotificationHubPushNotificationService> sutProvider, Notification notification,
        NotificationStatus notificationStatus)
    {
        notification.UserId = null;
        notification.ClientType = ClientType.All;
        var expectedSyncNotification = ToSyncNotificationPushNotification(notification, notificationStatus);

        await sutProvider.Sut.PushSyncNotificationStatusAsync(notification, notificationStatus);

        await AssertSendTemplateNotificationAsync(sutProvider, PushType.SyncNotificationStatus,
            expectedSyncNotification,
            $"(template:payload && organizationId:{notification.OrganizationId})");
        await sutProvider.GetDependency<IInstallationDeviceRepository>()
            .Received(0)
            .UpsertAsync(Arg.Any<InstallationDeviceEntity>());
    }

    [Theory]
    [BitAutoData(ClientType.Browser)]
    [BitAutoData(ClientType.Desktop)]
    [BitAutoData(ClientType.Web)]
    [BitAutoData(ClientType.Mobile)]
    [NotificationCustomize(false)]
    public async Task
        PushSyncNotificationStatusAsync_UserIdNullOrganizationIdProvidedClientTypeNotAll_SentToOrganization(
            ClientType clientType, SutProvider<NotificationHubPushNotificationService> sutProvider,
            Notification notification, NotificationStatus notificationStatus)
    {
        notification.UserId = null;
        notification.ClientType = clientType;
        var expectedSyncNotification = ToSyncNotificationPushNotification(notification, notificationStatus);

        await sutProvider.Sut.PushSyncNotificationStatusAsync(notification, notificationStatus);

        await AssertSendTemplateNotificationAsync(sutProvider, PushType.SyncNotificationStatus,
            expectedSyncNotification,
            $"(template:payload && organizationId:{notification.OrganizationId} && clientType:{clientType})");
        await sutProvider.GetDependency<IInstallationDeviceRepository>()
            .Received(0)
            .UpsertAsync(Arg.Any<InstallationDeviceEntity>());
    }

    [Theory]
    [BitAutoData([null])]
    [BitAutoData(ClientType.All)]
    public async Task SendPayloadToUserAsync_ClientTypeNullOrAll_SentToUser(ClientType? clientType,
        SutProvider<NotificationHubPushNotificationService> sutProvider, Guid userId, PushType pushType, string payload,
        string identifier)
    {
        await sutProvider.Sut.SendPayloadToUserAsync(userId.ToString(), pushType, payload, identifier, null,
            clientType);

        await AssertSendTemplateNotificationAsync(sutProvider, pushType, payload,
            $"(template:payload_userId:{userId} && !deviceIdentifier:{identifier})");
        await sutProvider.GetDependency<IInstallationDeviceRepository>()
            .Received(0)
            .UpsertAsync(Arg.Any<InstallationDeviceEntity>());
    }

    [Theory]
    [BitAutoData(ClientType.Browser)]
    [BitAutoData(ClientType.Desktop)]
    [BitAutoData(ClientType.Mobile)]
    [BitAutoData(ClientType.Web)]
    public async Task SendPayloadToUserAsync_ClientTypeExplicit_SentToUserAndClientType(ClientType clientType,
        SutProvider<NotificationHubPushNotificationService> sutProvider, Guid userId, PushType pushType, string payload,
        string identifier)
    {
        await sutProvider.Sut.SendPayloadToUserAsync(userId.ToString(), pushType, payload, identifier, null,
            clientType);

        await AssertSendTemplateNotificationAsync(sutProvider, pushType, payload,
            $"(template:payload_userId:{userId} && !deviceIdentifier:{identifier} && clientType:{clientType})");
        await sutProvider.GetDependency<IInstallationDeviceRepository>()
            .Received(0)
            .UpsertAsync(Arg.Any<InstallationDeviceEntity>());
    }

    [Theory]
    [BitAutoData([null])]
    [BitAutoData(ClientType.All)]
    public async Task SendPayloadToOrganizationAsync_ClientTypeNullOrAll_SentToOrganization(ClientType? clientType,
        SutProvider<NotificationHubPushNotificationService> sutProvider, Guid organizationId, PushType pushType,
        string payload, string identifier)
    {
        await sutProvider.Sut.SendPayloadToOrganizationAsync(organizationId.ToString(), pushType, payload, identifier,
            null, clientType);

        await AssertSendTemplateNotificationAsync(sutProvider, pushType, payload,
            $"(template:payload && organizationId:{organizationId} && !deviceIdentifier:{identifier})");
        await sutProvider.GetDependency<IInstallationDeviceRepository>()
            .Received(0)
            .UpsertAsync(Arg.Any<InstallationDeviceEntity>());
    }

    [Theory]
    [BitAutoData(ClientType.Browser)]
    [BitAutoData(ClientType.Desktop)]
    [BitAutoData(ClientType.Mobile)]
    [BitAutoData(ClientType.Web)]
    public async Task SendPayloadToOrganizationAsync_ClientTypeExplicit_SentToOrganizationAndClientType(
        ClientType clientType, SutProvider<NotificationHubPushNotificationService> sutProvider, Guid organizationId,
        PushType pushType, string payload, string identifier)
    {
        await sutProvider.Sut.SendPayloadToOrganizationAsync(organizationId.ToString(), pushType, payload, identifier,
            null, clientType);

        await AssertSendTemplateNotificationAsync(sutProvider, pushType, payload,
            $"(template:payload && organizationId:{organizationId} && !deviceIdentifier:{identifier} && clientType:{clientType})");
        await sutProvider.GetDependency<IInstallationDeviceRepository>()
            .Received(0)
            .UpsertAsync(Arg.Any<InstallationDeviceEntity>());
    }

    private static SyncNotificationPushNotification ToSyncNotificationPushNotification(Notification notification,
        NotificationStatus? notificationStatus) =>
        new()
        {
            Id = notification.Id,
            UserId = notification.UserId,
            OrganizationId = notification.OrganizationId,
            ClientType = notification.ClientType,
            RevisionDate = notification.RevisionDate,
            ReadDate = notificationStatus?.ReadDate,
            DeletedDate = notificationStatus?.DeletedDate
        };

    private static async Task AssertSendTemplateNotificationAsync(
        SutProvider<NotificationHubPushNotificationService> sutProvider, PushType type, object payload, string tag)
    {
        await sutProvider.GetDependency<INotificationHubPool>()
            .Received(1)
            .AllClients
            .Received(1)
            .SendTemplateNotificationAsync(
                Arg.Is<IDictionary<string, string>>(dictionary => MatchingSendPayload(dictionary, type, payload)),
                tag);
    }

    private static bool MatchingSendPayload(IDictionary<string, string> dictionary, PushType type, object payload)
    {
        return dictionary.ContainsKey("type") && dictionary["type"].Equals(((byte)type).ToString()) &&
               dictionary.ContainsKey("payload") && dictionary["payload"].Equals(JsonSerializer.Serialize(payload));
    }
}
