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
            entity.Property(e => e.Id).ValueGeneratedOnAdd(); 

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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
