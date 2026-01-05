using Locked_IN_Backend.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Locked_IN_Backend.Data;

//#TODO maybe it is better to separate the identity context from appDbContext
public partial class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<Chatparticipant> Chatparticipants { get; set; }

    public virtual DbSet<ExperienceTag> ExperienceTags { get; set; }

    public virtual DbSet<Friendship> Friendships { get; set; }
    
    public virtual DbSet<FriendshipStatus> FriendshipStatuses { get; set; }

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
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("chat_pk");
            entity.Property(e => e.Id).ValueGeneratedOnAdd(); 
        });

        modelBuilder.Entity<Chatparticipant>(entity =>
        {
            entity.HasKey(e => e.ChatparticipantId).HasName("chatparticipant_pk");
            entity.Property(e => e.ChatparticipantId).ValueGeneratedOnAdd(); 
        });

        modelBuilder.Entity<ExperienceTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("experience_tag_pk");
            entity.Property(e => e.Id).ValueGeneratedOnAdd(); 
        });

        modelBuilder.Entity<Friendship>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("friendship_pk");
            entity.Property(e => e.Id).ValueGeneratedOnAdd(); 

            entity.HasOne(d => d.User2).WithMany(p => p.FriendshipUser2s)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("friendship_user_2");

            entity.HasOne(d => d.User).WithMany(p => p.FriendshipUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("friendship_user_1");
            
            entity.HasOne(d => d.Status).WithMany(p => p.Friendships)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("friendship_status");
        });
        
        modelBuilder.Entity<FriendshipStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("friendship_status_pk");
            entity.Property(e => e.Id).ValueGeneratedNever(); 
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("game_pk");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<GameExp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("game_exp_pk");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<GameProfile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("game_profile_pk");
            entity.Property(e => e.Id).ValueGeneratedOnAdd(); 

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
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

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
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<GameprofilePreferencetagRelation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("gameprofile_preferencetag_relation_pk");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

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
            entity.Property(e => e.Id).ValueGeneratedOnAdd(); 

            entity.HasOne(d => d.ChatparticipantChatparticipant).WithMany(p => p.Messages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("message_chatparticipant");
        });

        modelBuilder.Entity<PreferenceTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("preference_tag_pk");
            entity.Property(e => e.Id).ValueGeneratedOnAdd(); 
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("role_pk");
            entity.Property(e => e.Id).ValueGeneratedOnAdd(); 
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("team_pk");
            entity.Property(e => e.Id).ValueGeneratedOnAdd(); 

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
            entity.Property(e => e.Id).UseIdentityColumn();


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
            entity.Property(e => e.Id).ValueGeneratedOnAdd(); 

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
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        SeedTestData(modelBuilder);

    }

private void SeedTestData(ModelBuilder modelBuilder)
{
    var hasher = new PasswordHasher<User>();
    string pepper = "LockedInSecretPepper2026";

    modelBuilder.Entity<Game>().HasData(
        new Game { Id = 1, Name = "Counter-Strike 2" },
        new Game { Id = 2, Name = "League of Legends" },
        new Game { Id = 3, Name = "Valorant" }
    );

    modelBuilder.Entity<ExperienceTag>().HasData(
        new ExperienceTag { Id = 1, Experiencelevel = "Beginner" },
        new ExperienceTag { Id = 2, Experiencelevel = "Intermediate" },
        new ExperienceTag { Id = 3, Experiencelevel = "Advanced" },
        new ExperienceTag { Id = 4, Experiencelevel = "Professional" }
    );

    modelBuilder.Entity<PreferenceTag>().HasData(
        new PreferenceTag { Id = 1, Name = "Competitive" },
        new PreferenceTag { Id = 2, Name = "Casual" },
        new PreferenceTag { Id = 3, Name = "Communication" },
        new PreferenceTag { Id = 4, Name = "Strategy Focus" },
        new PreferenceTag { Id = 5, Name = "Fun First" },
        new PreferenceTag { Id = 6, Name = "Skill Development" }
    );
    
    modelBuilder.Entity<MemberStatus>().HasData(
        new MemberStatus { Id = 1, Statusname = "Leader" },
        new MemberStatus { Id = 2, Statusname = "Member" },
        new MemberStatus { Id = 3, Statusname = "Pending" },
        new MemberStatus { Id = 4, Statusname = "Rejected" }
    );
    
    modelBuilder.Entity<FriendshipStatus>().HasData(
        new FriendshipStatus { Id = 1, StatusName = "Pending" },
        new FriendshipStatus { Id = 2, StatusName = "Accepted" },
        new FriendshipStatus { Id = 3, StatusName = "Blocked" }
    );
    
    modelBuilder.Entity<GameExp>().HasData(
        new GameExp { Id = 1, Experience = "< 100 hours" },
        new GameExp { Id = 2, Experience = "100-500 hours" },
        new GameExp { Id = 3, Experience = "500-1000 hours" },
        new GameExp { Id = 4, Experience = "1000+ hours" }
    );
    
    modelBuilder.Entity<GameplayPref>().HasData(
        new GameplayPref { Id = 1, Preference = "Voice Chat Only" },
        new GameplayPref { Id = 2, Preference = "Ping Only" },
        new GameplayPref { Id = 3, Preference = "Any Communication" }
    );

    modelBuilder.Entity<Role>().HasData(
        new Role { Id = 1, Rolename = "Member" },
        new Role { Id = 2, Rolename = "Admin" },
        new Role { Id = 3, Rolename = "Moderator" }
    );

    modelBuilder.Entity<User>().HasData(
            new User 
            { 
                Id = 1, 
                Email = "john.doe@example.com", 
                Nickname = "JohnDoe", 
                PasswordHash = hasher.HashPassword(null!, "password1" + pepper),
                AvatarUrl = "https://example.com/avatars/john.png",
                Availability = "{\"monday\": [\"18:00\", \"22:00\"], \"friday\": [\"19:00\", \"23:00\"]}",
                ConcurrencyStamp = "6f9098be-b650-4411-a000-5eb43a9552bb"
            },
            new User 
            { 
                Id = 2, 
                Email = "jane.smith@example.com", 
                Nickname = "JaneSmith", 
                PasswordHash = hasher.HashPassword(null!, "password2" + pepper),
                AvatarUrl = null,
                Availability = "{\"tuesday\": [\"17:00\", \"21:00\"], \"saturday\": [\"14:00\", \"18:00\"]}",
                ConcurrencyStamp = "cdf5a155-7689-41bb-899d-082da450358d"
            },
            new User 
            { 
                Id = 3, 
                Email = "mike.wilson@example.com", 
                Nickname = "MikeWilson", 
                PasswordHash = hasher.HashPassword(null!, "password3" + pepper),
                AvatarUrl = null,
                Availability = "{\"wednesday\": [\"20:00\", \"24:00\"], \"sunday\": [\"16:00\", \"20:00\"]}",
                ConcurrencyStamp = "7215511a-a6f6-4c13-9150-4adcd0386aa2"
            },
            new User 
            { 
                Id = 4, 
                Email = "sarah.johnson@example.com", 
                Nickname = "SarahJ", 
                PasswordHash = hasher.HashPassword(null!, "password4" + pepper),
                AvatarUrl = null,
                Availability = "{\"thursday\": [\"19:00\", \"23:00\"], \"saturday\": [\"15:00\", \"19:00\"]}",
                ConcurrencyStamp = "d88cb586-3c32-4568-8926-bc13615faee5"
            },
            new User 
            { 
                Id = 5, 
                Email = "test.user5@example.com", 
                Nickname = "TestUser5", 
                PasswordHash = hasher.HashPassword(null!, "password5" + pepper),
                AvatarUrl = null,
                Availability = "{}",
                ConcurrencyStamp = "72731087-823f-47c7-914a-c94b877b812b"
            },
            new User 
            { 
                Id = 6, 
                Email = "test.user6@example.com", 
                Nickname = "TestUser6", 
                PasswordHash = hasher.HashPassword(null!, "password6" + pepper),
                AvatarUrl = null,
                Availability = "{}",
                ConcurrencyStamp = "cfdd42d5-0600-4945-a36f-17b0f22459f4"
            }
        );

    modelBuilder.Entity<Team>().HasData(
        new Team
        {
            Id = 1,
            Name = "CS2 Legends",
            MinCompScore = 1500,
            MaxPlayerCount = 5,
            Description = "TestDescription1",
            GameId = 1,
            Isprivate = false,
            Isblitz = false,
            ExperienceTagId = 3,
            CreationTimestamp = new DateTime(2024, 9, 28, 10, 0, 0, DateTimeKind.Utc),
            IconUrl = "https://pl.wikipedia.org/wiki/World_of_Warcraft#/media/Plik:WoW_icon.svg"
        },
        new Team
        {
            Id = 2,
            Name = "LoL Rookies",
            MinCompScore = 800,
            MaxPlayerCount = 5,
            Description = "TestDescription2",
            GameId = 2,
            Isprivate = true,
            Isblitz = true,
            ExperienceTagId = 1,
            CreationTimestamp = new DateTime(2024, 10, 1, 12, 0, 0, DateTimeKind.Utc),
            IconUrl = "https://pl.wikipedia.org/wiki/World_of_Warcraft#/media/Plik:WoW_icon.svg"
        },
        new Team
        {
            Id = 3,
            Name = "Valorant Pros",
            MinCompScore = 2000,
            MaxPlayerCount = 5,
            Description = "TestDescription3",
            GameId = 3,
            Isprivate = false,
            Isblitz = false,
            ExperienceTagId = 4,
            CreationTimestamp = new DateTime(2024, 10, 5, 15, 0, 0, DateTimeKind.Utc),
            IconUrl = "https://pl.wikipedia.org/wiki/World_of_Warcraft#/media/Plik:WoW_icon.svg"
        },
        new Team
        {
            Id = 4,
            Name = "Casual Gamers",
            MinCompScore = null,
            MaxPlayerCount = 6,
            Description = "TestDescription4",
            GameId = 2,
            Isprivate = false,
            Isblitz = true,
            ExperienceTagId = 2,
            CreationTimestamp = new DateTime(2024, 10, 10, 18, 0, 0, DateTimeKind.Utc),
            IconUrl = "https://pl.wikipedia.org/wiki/World_of_Warcraft#/media/Plik:WoW_icon.svg"
        },
        new Team
        {
            Id = 5,
            Name = "Aim Squad",
            MinCompScore = 1200,
            MaxPlayerCount = 5,
            Description = "TestDescription5",
            GameId = 1,
            Isprivate = false,
            Isblitz = true,
            ExperienceTagId = 2,
            CreationTimestamp = new DateTime(2024, 9, 15, 19, 30, 0, DateTimeKind.Utc),
            IconUrl = "https://pl.wikipedia.org/wiki/World_of_Warcraft#/media/Plik:WoW_icon.svg"
        },
        new Team
        {
            Id = 6,
            Name = "Nexus Five",
            MinCompScore = 900,
            MaxPlayerCount = 5,
            Description = "TestDescription6",
            GameId = 2,
            Isprivate = true,
            Isblitz = false,
            ExperienceTagId = 1,
            CreationTimestamp = new DateTime(2024, 8, 30, 8, 45, 0, DateTimeKind.Utc),
            IconUrl = "https://pl.wikipedia.org/wiki/World_of_Warcraft#/media/Plik:WoW_icon.svg"
        },
        new Team
        {
            Id = 7,
            Name = "VLR Strike",
            MinCompScore = 1800,
            MaxPlayerCount = 5,
            Description = "TestDescription7",
            GameId = 3,
            Isprivate = false,
            Isblitz = false,
            ExperienceTagId = 3,
            CreationTimestamp = new DateTime(2024, 10, 2, 21, 10, 0, DateTimeKind.Utc),
            IconUrl = "https://pl.wikipedia.org/wiki/World_of_Warcraft#/media/Plik:WoW_icon.svg"
        },
        new Team
        {
            Id = 8,
            Name = "Chill Queue",
            MinCompScore = null,
            MaxPlayerCount = 5,
            Description = "TestDescription8",
            GameId = 2,
            Isprivate = false,
            Isblitz = false,
            ExperienceTagId = 1,
            CreationTimestamp = new DateTime(2024, 9, 5, 13, 5, 0, DateTimeKind.Utc),
            IconUrl = "https://pl.wikipedia.org/wiki/World_of_Warcraft#/media/Plik:WoW_icon.svg"
        },
        new Team
        {
            Id = 9,
            Name = "Peak Hold",
            MinCompScore = 1600,
            MaxPlayerCount = 5,
            Description = "TestDescription9",
            GameId = 1,
            Isprivate = true,
            Isblitz = true,
            ExperienceTagId = 4,
            CreationTimestamp = new DateTime(2024, 11, 1, 9, 0, 0, DateTimeKind.Utc),
            IconUrl = "https://pl.wikipedia.org/wiki/World_of_Warcraft#/media/Plik:WoW_icon.svg"
        },
        new Team
        {
            Id = 10,
            Name = "Spike Rush",
            MinCompScore = 1100,
            MaxPlayerCount = 5,
            Description = "TestDescription10",
            GameId = 3,
            Isprivate = false,
            Isblitz = true,
            ExperienceTagId = 2,
            CreationTimestamp = new DateTime(2024, 8, 20, 17, 25, 0, DateTimeKind.Utc),
            IconUrl = "https://pl.wikipedia.org/wiki/World_of_Warcraft#/media/Plik:WoW_icon.svg"
        }
    );

    modelBuilder.Entity<TeamMember>().HasData(
        new TeamMember
        {
            Id = 1,
            Isleader = true,
            Jointimestamp = new DateTime(2024, 9, 28, 10, 0, 0, DateTimeKind.Utc),
            TeamId = 1,
            UserId = 1,
            MemberStatusId = 1
        },
        new TeamMember
        {
            Id = 2,
            Isleader = false,
            Jointimestamp = new DateTime(2024, 10, 3, 14, 30, 0, DateTimeKind.Utc),
            TeamId = 1,
            UserId = 2,
            MemberStatusId = 2
        },
        new TeamMember
        {
            Id = 3,
            Isleader = true,
            Jointimestamp = new DateTime(2024, 10, 8, 16, 15, 0, DateTimeKind.Utc),
            TeamId = 2,
            UserId = 3,
            MemberStatusId = 1
        },
        new TeamMember
        {
            Id = 4,
            Isleader = false,
            Jointimestamp = new DateTime(2024, 10, 13, 11, 45, 0, DateTimeKind.Utc),
            TeamId = 2,
            UserId = 4,
            MemberStatusId = 3
        },
        new TeamMember
        {
            Id = 5,
            Isleader = true,
            Jointimestamp = new DateTime(2024, 10, 18, 9, 20, 0, DateTimeKind.Utc),
            TeamId = 3,
            UserId = 1,
            MemberStatusId = 1
        },
        new TeamMember
        {
            Id = 6,
            Isleader = true,
            Jointimestamp = new DateTime(2024, 10, 23, 18, 0, 0, DateTimeKind.Utc),
            TeamId = 4,
            UserId = 2,
            MemberStatusId = 1
        },
        new TeamMember
        {
            Id = 7,
            Isleader = true,
            Jointimestamp = new DateTime(2024, 9, 16, 10, 0, 0, DateTimeKind.Utc),
            TeamId = 5,
            UserId = 3,
            MemberStatusId = 1
        },
        new TeamMember
        {
            Id = 8,
            Isleader = true,
            Jointimestamp = new DateTime(2024, 9, 1, 12, 30, 0, DateTimeKind.Utc),
            TeamId = 6,
            UserId = 4,
            MemberStatusId = 1
        },
        new TeamMember
        {
            Id = 9,
            Isleader = true,
            Jointimestamp = new DateTime(2024, 10, 3, 20, 15, 0, DateTimeKind.Utc),
            TeamId = 7,
            UserId = 5,
            MemberStatusId = 1
        },
        new TeamMember
        {
            Id = 10,
            Isleader = true,
            Jointimestamp = new DateTime(2024, 9, 6, 9, 5, 0, DateTimeKind.Utc),
            TeamId = 8,
            UserId = 6,
            MemberStatusId = 1
        },
        new TeamMember
        {
            Id = 11,
            Isleader = true,
            Jointimestamp = new DateTime(2024, 11, 2, 10, 0, 0, DateTimeKind.Utc),
            TeamId = 9,
            UserId = 1,
            MemberStatusId = 1
        },
        new TeamMember
        {
            Id = 12,
            Isleader = true,
            Jointimestamp = new DateTime(2024, 8, 21, 18, 0, 0, DateTimeKind.Utc),
            TeamId = 10,
            UserId = 2,
            MemberStatusId = 1
        }
    );


    modelBuilder.Entity<TeamPreferencetagRelation>().HasData(
        new TeamPreferencetagRelation { Id = 1, TeamId = 1, PreferenceTagId = 1 },
        new TeamPreferencetagRelation { Id = 2, TeamId = 1, PreferenceTagId = 3 },

        new TeamPreferencetagRelation { Id = 3, TeamId = 2, PreferenceTagId = 2 },
        new TeamPreferencetagRelation { Id = 4, TeamId = 2, PreferenceTagId = 5 },

        new TeamPreferencetagRelation { Id = 5, TeamId = 3, PreferenceTagId = 1 },
        new TeamPreferencetagRelation { Id = 6, TeamId = 3, PreferenceTagId = 4 },

        new TeamPreferencetagRelation { Id = 7, TeamId = 4, PreferenceTagId = 2 },
        new TeamPreferencetagRelation { Id = 8, TeamId = 4, PreferenceTagId = 6 },

        new TeamPreferencetagRelation { Id = 9, TeamId = 5, PreferenceTagId = 1 },
        new TeamPreferencetagRelation { Id = 10, TeamId = 5, PreferenceTagId = 6 },

        new TeamPreferencetagRelation { Id = 11, TeamId = 6, PreferenceTagId = 2 },
        new TeamPreferencetagRelation { Id = 12, TeamId = 6, PreferenceTagId = 3 },

        new TeamPreferencetagRelation { Id = 13, TeamId = 7, PreferenceTagId = 4 },
        new TeamPreferencetagRelation { Id = 14, TeamId = 7, PreferenceTagId = 1 },

        new TeamPreferencetagRelation { Id = 15, TeamId = 8, PreferenceTagId = 5 },
        new TeamPreferencetagRelation { Id = 16, TeamId = 8, PreferenceTagId = 2 },

        new TeamPreferencetagRelation { Id = 17, TeamId = 9, PreferenceTagId = 3 },
        new TeamPreferencetagRelation { Id = 18, TeamId = 9, PreferenceTagId = 6 },

        new TeamPreferencetagRelation { Id = 19, TeamId = 10, PreferenceTagId = 4 },
        new TeamPreferencetagRelation { Id = 20, TeamId = 10, PreferenceTagId = 1 }
    );
    
    modelBuilder.Entity<Friendship>().HasData(
        new Friendship 
        { 
            Id = 1, 
            UserId = 1, 
            User2Id = 2, 
            StatusId = 2, 
            RequestTimestamp = new DateTime(2024, 10, 1, 0, 0, 0, DateTimeKind.Utc)
        },
        new Friendship 
        { 
            Id = 2, 
            UserId = 3, 
            User2Id = 1, 
            StatusId = 1, 
            RequestTimestamp = new DateTime(2024, 10, 5, 0, 0, 0, DateTimeKind.Utc)
        }
    );
}
    
}