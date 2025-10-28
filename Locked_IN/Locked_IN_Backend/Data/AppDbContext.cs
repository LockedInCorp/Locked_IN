using System;
using System.Collections.Generic;
using Locked_IN_Backend.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<Chatparticipant> Chatparticipants { get; set; }

    public virtual DbSet<ExperienceTag> ExperienceTags { get; set; }

    public virtual DbSet<Friendship> Friendships { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GameExp> GameExps { get; set; }

    public virtual DbSet<GameProfile> GameProfiles { get; set; }

    public virtual DbSet<GameProfilePref> GameProfilePrefs { get; set; }

    public virtual DbSet<GameplayPref> GameplayPrefs { get; set; }

    public virtual DbSet<GameprofilePreferencetagRelation> GameprofilePreferencetagRelations { get; set; }

    public virtual DbSet<MemberStatus> MemberStatuses { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<PreferenceTag> PreferenceTags { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<TeamMember> TeamMembers { get; set; }

    public virtual DbSet<TeamPreferencetagRelation> TeamPreferencetagRelations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("chat_pk");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Team).WithMany(p => p.Chats).HasConstraintName("chat_team");
        });

        modelBuilder.Entity<Chatparticipant>(entity =>
        {
            entity.HasKey(e => e.ChatparticipantId).HasName("chatparticipant_pk");

            entity.Property(e => e.ChatparticipantId).ValueGeneratedNever();

            entity.HasOne(d => d.Chat).WithMany(p => p.Chatparticipants)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chatparticipant_chat");

            entity.HasOne(d => d.Role).WithMany(p => p.Chatparticipants)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chatparticipant_role");

            entity.HasOne(d => d.User).WithMany(p => p.Chatparticipants)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("table_9_user");
        });

        modelBuilder.Entity<ExperienceTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("experience_tag_pk");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Friendship>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("friendship_pk");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.User2).WithMany(p => p.FriendshipUser2s)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("friendship_user_2");

            entity.HasOne(d => d.User).WithMany(p => p.FriendshipUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("friendship_user_1");
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("game_pk");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<GameExp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("game_exp_pk");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<GameProfile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("game_profile_pk");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.ExperienceTag).WithMany(p => p.GameProfiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("game_profile_experience_tag");

            entity.HasOne(d => d.GameExp).WithMany(p => p.GameProfiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("game_profile_game_exp");

            entity.HasOne(d => d.Game).WithMany(p => p.GameProfiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_game_game");

            entity.HasOne(d => d.User).WithMany(p => p.GameProfiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_game_user");
        });

        modelBuilder.Entity<GameProfilePref>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("game_profile_pref_pk");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.GameProfile).WithMany(p => p.GameProfilePrefs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("game_profile_pref_game_profile");

            entity.HasOne(d => d.GameplayPref).WithMany(p => p.GameProfilePrefs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("game_profile_pref_gameplay_pref");
        });

        modelBuilder.Entity<GameplayPref>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("gameplay_pref_pk");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<GameprofilePreferencetagRelation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("gameprofile_preferencetag_relation_pk");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.GameProfile).WithMany(p => p.GameprofilePreferencetagRelations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("gameprofile_preferencetag_relation_game_profile");

            entity.HasOne(d => d.PreferenceTag).WithMany(p => p.GameprofilePreferencetagRelations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("gameprofile_preferencetag_relation_preference_tag");
        });

        modelBuilder.Entity<MemberStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("member_status_pk");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("message_pk");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.ChatparticipantChatparticipant).WithMany(p => p.Messages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("message_chatparticipant");
        });

        modelBuilder.Entity<PreferenceTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("preference_tag_pk");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("role_pk");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("team_pk");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.ExperienceTag).WithMany(p => p.Teams)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("team_experience_tag");

            entity.HasOne(d => d.Game).WithMany(p => p.Teams)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("team_game");
        });

        modelBuilder.Entity<TeamMember>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("team_member_pk");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.MemberStatus).WithMany(p => p.TeamMembers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("team_member_status");

            entity.HasOne(d => d.Team).WithMany(p => p.TeamMembers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("team_member_team");

            entity.HasOne(d => d.User).WithMany(p => p.TeamMembers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("team_member_user");
        });

        modelBuilder.Entity<TeamPreferencetagRelation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("team_preferencetag_relation_pk");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.PreferenceTag).WithMany(p => p.TeamPreferencetagRelations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("team_preferencetag_relation_preference_tag");

            entity.HasOne(d => d.Team).WithMany(p => p.TeamPreferencetagRelations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("team_preferencetag_relation_team");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_pk");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
        SeedTestData(modelBuilder);

    }

    private void SeedTestData(ModelBuilder modelBuilder) 
    {
    // Seed Games
    modelBuilder.Entity<Game>().HasData(
        new Game { Id = 1, Name = "Counter-Strike 2" },
        new Game { Id = 2, Name = "League of Legends" },
        new Game { Id = 3, Name = "Valorant" }
    );

    // Seed Experience Tags
    modelBuilder.Entity<ExperienceTag>().HasData(
        new ExperienceTag { Id = 1, Experiencelevel = "Beginner" },
        new ExperienceTag { Id = 2, Experiencelevel = "Intermediate" },
        new ExperienceTag { Id = 3, Experiencelevel = "Advanced" },
        new ExperienceTag { Id = 4, Experiencelevel = "Professional" }
    );

    // Seed Preference Tags
    modelBuilder.Entity<PreferenceTag>().HasData(
        new PreferenceTag { Id = 1, Preferencename = "Competitive" },
        new PreferenceTag { Id = 2, Preferencename = "Casual" },
        new PreferenceTag { Id = 3, Preferencename = "Communication" },
        new PreferenceTag { Id = 4, Preferencename = "Strategy Focus" },
        new PreferenceTag { Id = 5, Preferencename = "Fun First" },
        new PreferenceTag { Id = 6, Preferencename = "Skill Development" }
    );

    // Seed Member Status
    modelBuilder.Entity<MemberStatus>().HasData(
        new MemberStatus { Id = 1, Statusname = "Active" },
        new MemberStatus { Id = 2, Statusname = "Pending" },
        new MemberStatus { Id = 3, Statusname = "Inactive" }
    );

    // Seed Test Users
    modelBuilder.Entity<User>().HasData(
        new User 
        { 
            Id = 1, 
            Email = "john.doe@example.com", 
            Nickname = "JohnDoe", 
            HashedPass = "hashed_password_1",
            Availability = "{\"monday\": [\"18:00\", \"22:00\"], \"friday\": [\"19:00\", \"23:00\"]}"
        },
        new User 
        { 
            Id = 2, 
            Email = "jane.smith@example.com", 
            Nickname = "JaneSmith", 
            HashedPass = "hashed_password_2",
            Availability = "{\"tuesday\": [\"17:00\", \"21:00\"], \"saturday\": [\"14:00\", \"18:00\"]}"
        },
        new User 
        { 
            Id = 3, 
            Email = "mike.wilson@example.com", 
            Nickname = "MikeWilson", 
            HashedPass = "hashed_password_3",
            Availability = "{\"wednesday\": [\"20:00\", \"24:00\"], \"sunday\": [\"16:00\", \"20:00\"]}"
        },
        new User 
        { 
            Id = 4, 
            Email = "sarah.johnson@example.com", 
            Nickname = "SarahJ", 
            HashedPass = "hashed_password_4",
            Availability = "{\"thursday\": [\"19:00\", \"23:00\"], \"saturday\": [\"15:00\", \"19:00\"]}"
        }
    );

    // Seed Test Teams
    modelBuilder.Entity<Team>().HasData(
        new Team
        {
            Id = 1,
            Name = "CS2 Legends",
            MinCompScore = 1500,
            MaxPlayerCount = 5,
            Description = 101, // Assuming this should be a text/description ID
            GameId = 1,
            Isprivate = false,
            Isblitz = false,
            ExperienceTagId = 3
        },
        new Team
        {
            Id = 2,
            Name = "LoL Rookies",
            MinCompScore = 800,
            MaxPlayerCount = 5,
            Description = 102,
            GameId = 2,
            Isprivate = true,
            Isblitz = true,
            ExperienceTagId = 1
        },
        new Team
        {
            Id = 3,
            Name = "Valorant Pros",
            MinCompScore = 2000,
            MaxPlayerCount = 5,
            Description = 103,
            GameId = 3,
            Isprivate = false,
            Isblitz = false,
            ExperienceTagId = 4
        },
        new Team
        {
            Id = 4,
            Name = "Casual Gamers",
            MinCompScore = null,
            MaxPlayerCount = 6,
            Description = 104,
            GameId = 2,
            Isprivate = false,
            Isblitz = true,
            ExperienceTagId = 2
        }
    );

    // Seed Team Members
    modelBuilder.Entity<TeamMember>().HasData(
        // Team 1 members
        new TeamMember
        {
            Id = 1,
            Isleader = true,
            Jointimestamp = new DateTime(2024, 9, 28, 10, 0, 0, DateTimeKind.Utc), // 30 days ago from Oct 28, 2024
            TeamId = 1,
            UserId = 1,
            MemberStatusId = 1
        },
        new TeamMember
        {
            Id = 2,
            Isleader = false,
            Jointimestamp = new DateTime(2024, 10, 3, 14, 30, 0, DateTimeKind.Utc), // 25 days ago
            TeamId = 1,
            UserId = 2,
            MemberStatusId = 1
        },
        // Team 2 members
        new TeamMember
        {
            Id = 3,
            Isleader = true,
            Jointimestamp = new DateTime(2024, 10, 8, 16, 15, 0, DateTimeKind.Utc), // 20 days ago
            TeamId = 2,
            UserId = 3,
            MemberStatusId = 1
        },
        new TeamMember
        {
            Id = 4,
            Isleader = false,
            Jointimestamp = new DateTime(2024, 10, 13, 11, 45, 0, DateTimeKind.Utc), // 15 days ago
            TeamId = 2,
            UserId = 4,
            MemberStatusId = 2
        },
        // Team 3 members
        new TeamMember
        {
            Id = 5,
            Isleader = true,
            Jointimestamp = new DateTime(2024, 10, 18, 9, 20, 0, DateTimeKind.Utc), // 10 days ago
            TeamId = 3,
            UserId = 1,
            MemberStatusId = 1
        },
        // Team 4 members
        new TeamMember
        {
            Id = 6,
            Isleader = true,
            Jointimestamp = new DateTime(2024, 10, 23, 18, 0, 0, DateTimeKind.Utc), // 5 days ago
            TeamId = 4,
            UserId = 2,
            MemberStatusId = 1
        }
    );


    // Seed Team Preference Tag Relations
    modelBuilder.Entity<TeamPreferencetagRelation>().HasData(
        // Team 1 preferences
        new TeamPreferencetagRelation { Id = 1, TeamId = 1, PreferenceTagId = 1 }, // Competitive
        new TeamPreferencetagRelation { Id = 2, TeamId = 1, PreferenceTagId = 3 }, // Communication
        // Team 2 preferences  
        new TeamPreferencetagRelation { Id = 3, TeamId = 2, PreferenceTagId = 2 }, // Casual
        new TeamPreferencetagRelation { Id = 4, TeamId = 2, PreferenceTagId = 5 }, // Fun First
        // Team 3 preferences
        new TeamPreferencetagRelation { Id = 5, TeamId = 3, PreferenceTagId = 1 }, // Competitive
        new TeamPreferencetagRelation { Id = 6, TeamId = 3, PreferenceTagId = 4 }, // Strategy Focus
        // Team 4 preferences
        new TeamPreferencetagRelation { Id = 7, TeamId = 4, PreferenceTagId = 2 }, // Casual
        new TeamPreferencetagRelation { Id = 8, TeamId = 4, PreferenceTagId = 6 }  // Skill Development
    ); 
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    
}
