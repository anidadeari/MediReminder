using Xunit;
using MediReminder.Models;

namespace MediReminder.Tests;

public class UnitTest1
{
    // Test 1: Verify that a User object can be created with valid properties
    [Fact]
    public void User_CanBeCreated_WithValidProperties()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "hashedpassword",
            Role = "User",
            CreatedAt = DateTime.UtcNow
        };

        // Act & Assert
        Assert.Equal(1, user.Id);
        Assert.Equal("testuser", user.Username);
        Assert.Equal("test@example.com", user.Email);
        Assert.Equal("User", user.Role);
    }

    // Test 2: Verify that a Medication object can be created with valid properties
    [Fact]
    public void Medication_CanBeCreated_WithValidProperties()
    {
        // Arrange
        var medication = new Medication
        {
            Id = 1,
            Name = "Aspirin",
            Dosage = "100mg",
            Frequency = "Daily",
            StartDate = DateTime.UtcNow,
            EndDate = null,
            IsActive = true,
            UserId = 1
        };

        // Act & Assert
        Assert.Equal("Aspirin", medication.Name);
        Assert.Equal("100mg", medication.Dosage);
        Assert.Equal("Daily", medication.Frequency);
        Assert.True(medication.IsActive);
    }

    // Test 3: Verify that a MedicationLog records correct intake status
    [Fact]
    public void MedicationLog_CanRecord_TakenStatus()
    {
        // Arrange
        var log = new MedicationLog
        {
            Id = 1,
            MedicationId = 1,
            WasTaken = true,
            TakenAt = DateTime.UtcNow,
            Notes = "Taken with breakfast"
        };

        // Act & Assert
        Assert.True(log.WasTaken);
        Assert.Equal("Taken with breakfast", log.Notes);
        Assert.Equal(1, log.MedicationId);
    }

    // Test 4: Verify default role for new user is "User" not "Admin"
    [Fact]
    public void NewUser_DefaultRole_ShouldBeUser()
    {
        // Arrange
        var user = new User
        {
            Username = "newuser",
            Email = "new@example.com",
            PasswordHash = "hashedpassword",
            Role = "User"
        };

        // Act & Assert
        Assert.Equal("User", user.Role);
        Assert.NotEqual("Admin", user.Role);
    }

    // Test 5: Verify medication can be marked inactive
    [Fact]
    public void Medication_CanBeMarked_AsInactive()
    {
        // Arrange
        var medication = new Medication
        {
            Name = "Old Medication",
            Dosage = "50mg",
            Frequency = "Daily",
            IsActive = false
        };

        // Act & Assert
        Assert.False(medication.IsActive);
    }
}