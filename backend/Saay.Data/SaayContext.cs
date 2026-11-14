using Microsoft.EntityFrameworkCore;
using Saay.Data.Entities;

namespace Saay.Data;

public partial class SaayContext : DbContext
{
    public SaayContext()
    {
    }

    public SaayContext(DbContextOptions<SaayContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TaskCategory> TasksCategories { get; set; }

    public virtual DbSet<Goal> Goals { get; set; }

    public virtual DbSet<GoalCategory> GoalsCategories { get; set; }

    public virtual DbSet<Habit> Habits { get; set; }

    public DbSet<HabitLog> HabitsLogs { get; set; }

    public virtual DbSet<Saay.Data.Entities.Task> Tasks { get; set; }

    public virtual DbSet<Token> Tokens { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public DbSet<EmailConfirmationToken> EmailConfirmationTokens { get; set; }

    public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<UserGroup> UsersGroups { get; set; }

    public virtual DbSet<Challenge> Challenges { get; set; }

    public DbSet<GroupInvitationToken> GroupInvitationTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskCategory>(entity =>
        {
            entity.Property(e => e.TaskCategoryId).HasColumnName("TaskCategoryID");
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Goal>(entity =>
        {
            entity.Property(e => e.GoalId).HasColumnName("GoalID");
            entity.Property(e => e.GoalCategoryId).HasColumnName("GoalCategoryID");
            entity.Property(e => e.TimeFrame).HasComment("0- Monthly, 1- Quarterly, 2- Biannual, 3- Annually");
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.GoalCategory).WithMany(p => p.Goals)
                .HasForeignKey(d => d.GoalCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Goals_Categories");

            entity.HasOne(d => d.User).WithMany(p => p.Goals)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Goals_Users");
        });

        modelBuilder.Entity<GoalCategory>(entity =>
        {
            entity.ToTable("GoalsCategories");

            entity.Property(e => e.GoalCategoryId)
                .HasColumnName("GoalCategoryID");

            entity.Property(e => e.Title)
                .HasMaxLength(255);
        });

        modelBuilder.Entity<Habit>(entity =>
        {
            entity.Property(e => e.HabitId).HasColumnName("HabitID");
            entity.Property(e => e.ReasonForHabit).HasMaxLength(255);
            entity.Property(e => e.Steps).HasMaxLength(255);
            entity.Property(e => e.TargetDuration).HasComment("0- 30 days, 1- 60 days, 2- 90 days");
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.HabitStartDate)
            .HasColumnType("date")
            .HasDefaultValueSql("CONVERT([date], GETDATE())")
            .ValueGeneratedOnAdd();

            entity.HasOne(d => d.User).WithMany(p => p.Habits)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Habits_Users");
        });

        modelBuilder.Entity<HabitLog>(entity =>
        {
            entity.ToTable("HabitsLogs");

            entity.HasKey(log => log.HabitLogId);

            entity.Property(log => log.HabitLogId)
                .ValueGeneratedOnAdd();

            entity.HasOne(log => log.Habit)
                .WithMany(habit => habit.HabitLogs)
                .HasForeignKey(log => log.HabitId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Saay.Data.Entities.Task>(entity =>
        {
            entity.Property(e => e.TaskId).HasColumnName("TaskID");
            entity.Property(e => e.TaskCategoryId).HasColumnName("TaskCategoryID");
            entity.Property(e => e.DueDate).HasDefaultValueSql("(CONVERT([date],getdate()))");
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.Property(e => e.PrayerNumber)
               .HasColumnType("tinyint")
               .IsRequired(false);

            entity.ToTable("Tasks", table =>
                table.HasCheckConstraint(
                    "CK_Tasks_PrayerNumber",
                    "[PrayerNumber] >= 0 AND [PrayerNumber] <= 5 OR [PrayerNumber] IS NULL"));

            entity.HasOne(d => d.TaskCategory).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.TaskCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tasks_Categories");

            entity.HasOne(d => d.User).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Tasks_Users");

            entity.HasOne(d => d.Goal)
                .WithMany(p => p.Tasks)
                .HasForeignKey(d => d.GoalId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Tasks_Goals");
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity.Property(e => e.TokenId).HasColumnName("TokenID");
            entity.Property(e => e.ExpiresAt).HasColumnType("datetime");
            entity.Property(e => e.RefreshTokenHash)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RevokedAt).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Tokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Tokens_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Email, "UQ_Users_Email").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Mission).HasMaxLength(255);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ProfilePictureUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ProfilePictureURL");
            entity.Property(u => u.IsConfirmed)
                .HasDefaultValue(false);
        });


        modelBuilder.Entity<EmailConfirmationToken>(entity =>
        {
            entity.HasKey(e => e.EmailConfirmationTokenId);

            entity.Property(e => e.TokenHash)
                .IsRequired()
                .HasMaxLength(64)
                .IsUnicode(false);

            entity.HasIndex(e => e.TokenHash)
                .IsUnique();

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(e => e.ExpiresAt)
                .IsRequired();

            entity.Property(e => e.UsedAt)
                .IsRequired(false);
        });

        modelBuilder.Entity<PasswordResetToken>(entity =>
        {
            entity.ToTable("PasswordResetTokens", "dbo");

            entity.HasKey(e => e.PasswordResetTokenId);

            entity.Property(e => e.CodeHash)
                .IsRequired()
                .HasMaxLength(64)
                .IsUnicode(false);

            entity.Property(e => e.ExpiresAt)
                .HasColumnType("datetime2")
                .IsRequired();

            entity.Property(e => e.UsedAt)
                .HasColumnType("datetime2")
                .IsRequired(false);

            entity.HasIndex(e => e.UserId)
                .HasDatabaseName("IX_PasswordResetTokens_UserId");

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PasswordResetTokens_Users");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.ToTable("Groups");

            entity.HasKey(e => e.GroupId)
                .HasName("PK_Groups_GroupID");

            entity.Property(e => e.GroupId)
                .HasColumnName("GroupID")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Description)
                .HasMaxLength(200);

            entity.Property(e => e.CreatedAt)
                .HasColumnType("date")
                .HasDefaultValueSql("CAST(GETDATE() AS DATE)")
                .ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<UserGroup>(entity =>
        {
            entity.ToTable("UsersGroups");

            entity.HasKey(e => new { e.GroupId, e.UserId })
                .HasName("PK_UsersGroups_GroupID_UserID");

            entity.Property(e => e.GroupId)
                .HasColumnName("GroupID");

            entity.Property(e => e.UserId)
                .HasColumnName("UserID");

            entity.Property(e => e.Role)
                .HasColumnType("tinyint")
                .IsRequired();

            entity.HasOne(e => e.Group)
                .WithMany(g => g.UsersGroups)
                .HasForeignKey(e => e.GroupId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_UsersGroups_Groups");

            entity.HasOne(e => e.User)
                .WithMany(u => u.UsersGroups)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_UsersGroups_Users");
        });

        modelBuilder.Entity<Challenge>(entity =>
        {
            entity.ToTable("Challenges", table =>
                table.HasCheckConstraint(
                    "CK_Challenges_Dates",
                    "[EndDate] >= [StartDate]"));

            entity.HasKey(e => e.ChallengeId)
                .HasName("PK_Challenges_ChallengeID");

            entity.Property(e => e.ChallengeId)
                .HasColumnName("ChallengeID")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Description)
                .HasMaxLength(200);

            entity.Property(e => e.StartDate)
                .HasColumnType("date");

            entity.Property(e => e.EndDate)
                .HasColumnType("date");

            entity.Property(e => e.GroupId)
                .HasColumnName("GroupID");

            entity.HasOne(e => e.Group)
                .WithMany(g => g.Challenges)
                .HasForeignKey(e => e.GroupId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Challenges_Groups");
        });

        modelBuilder.Entity<GroupInvitationToken>(entity =>
        {
            entity.ToTable("GroupInvitationTokens");

            entity.HasKey(e => e.GroupInvitationTokenId);

            entity.Property(e => e.TokenHash)
                .IsRequired()
                .HasMaxLength(64)
                .IsUnicode(false);

            entity.HasIndex(e => e.TokenHash)
                .IsUnique();

            entity.Property(e => e.ExpiresAt)
                .HasColumnType("datetime2")
                .IsRequired();

            entity.Property(e => e.UsedAt)
                .HasColumnType("datetime2")
                .IsRequired(false);

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_GroupInvitationTokens_Users");

            entity.HasOne(e => e.Group)
                .WithMany()
                .HasForeignKey(e => e.GroupId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_GroupInvitationTokens_Groups");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
