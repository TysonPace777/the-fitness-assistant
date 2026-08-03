# Fitness Assistant Database Backup and Restore Test

## Purpose

This document explains how to verify that the automated PostgreSQL backup system works by restoring a backup file into the local PostgreSQL backup database on the Raspberry Pi.

This test does **not** modify the production Render database.

The restore target is:

```
Raspberry Pi PostgreSQL database:
fitness_assistant_backup
```

The backup source is:

```
Render PostgreSQL database:
the_fitness_assistant
```

---

# Prerequisites

Before testing restore:

- Raspberry Pi is powered on
- Raspberry Pi is connected to the network

---

THE FOLLOWING IS WHAT THE TERMINAL SHOULD GENERALLY LOOK LIKE IF YOU FOLLOW THESE INSTRUCTIONS CORRECTLY

holly@holly-pi:~ $ pwd
/home/holly
holly@holly-pi:~ $ ls -lh ~/db_backups
total 36K
-rw-rw-r-- 1 holly holly 9.0K Jul 22 17:37 fitness_backup_2026-07-22_17-37.sql
-rw-rw-r-- 1 holly holly 9.0K Jul 22 19:10 fitness_backup_2026-07-22_19-09.sql
-rw-rw-r-- 1 holly holly 9.0K Jul 22 19:21 fitness_backup_2026-07-22_19-21.sql
holly@holly-pi:~ $ psql -U fitness -d fitness_assistant_backup
Password for user fitness:
psql (18.4 (Debian 18.4-1.pgdg13+1), server 17.10 (Debian 17.10-0+deb13u1))
Type "help" for help.

fitness_assistant_backup=> DROP SCHEMA public CASCADE;
NOTICE: drop cascades to 5 other objects
DETAIL: drop cascades to table "CalorieGoals"
drop cascades to table "FoodLogEntries"
drop cascades to table "Foods"
drop cascades to table "Users"
drop cascades to table "\_\_EFMigrationsHistory"
DROP SCHEMA
fitness_assistant_backup=> CREATE SCHEMA public;
CREATE SCHEMA
fitness_assistant_backup=> \q
holly@holly-pi:~ $ psql -U fitness -d fitness_assistant_backup < ~/db_backups/fitness_backup_2026-07-22_19-21.sql
Password for user fitness:
SET
SET
SET
SET
SET
SET
set_config

---

(1 row)

SET
SET
SET
SET
SET
SET
CREATE TABLE
ALTER TABLE
CREATE TABLE
ALTER TABLE
CREATE TABLE
ALTER TABLE
CREATE TABLE
ALTER TABLE
CREATE TABLE
COPY 2
COPY 6
COPY 6
COPY 2
COPY 1
setval

---

      2

(1 row)

## setval

      6

(1 row)

## setval

      6

(1 row)

## setval

      2

(1 row)

ALTER TABLE
ALTER TABLE
ALTER TABLE
ALTER TABLE
ALTER TABLE
CREATE INDEX
CREATE INDEX
CREATE INDEX
CREATE INDEX
ALTER TABLE
ALTER TABLE
ALTER TABLE
ALTER TABLE
ERROR: role "the_fitness_assistant_user" does not exist
ERROR: role "the_fitness_assistant_user" does not exist
ERROR: role "the_fitness_assistant_user" does not exist
ERROR: role "the_fitness_assistant_user" does not exist
holly@holly-pi:~ $ psql -U fitness -d fitness_assistant_backup
Password for user fitness:
psql (18.4 (Debian 18.4-1.pgdg13+1), server 17.10 (Debian 17.10-0+deb13u1))
Type "help" for help.

fitness_assistant_backup=> \dt
List of tables
Schema | Name | Type | Owner
--------+-----------------------+-------+---------
public | CalorieGoals | table | fitness
public | FoodLogEntries | table | fitness
public | Foods | table | fitness
public | Users | table | fitness
public | \_\_EFMigrationsHistory | table | fitness
(5 rows)

fitness_assistant_backup=> SELECT \* FROM "Users";
UserId | Email | DisplayName | Height | Weight | Age
--------+----------------------------+--------------+--------+--------+-----
1 | hollybriggs1701e@gmail.com | Holly Briggs | 64 | 165 | 35
2 | 32.tpace@gmail.com | Tyson Pace | 64 | 165 | 35
(2 rows)

fitness_assistant_backup=> SELECT COUNT(\*) FROM "Foods";
count

---

     6

(1 row)

fitness_assistant_backup=> SELECT COUNT(\*) FROM "FoodLogEntries";
count

---

     6

(1 row)

fitness_assistant_backup=> \q
holly@holly-pi:~ $

---

Implemented an automated PostgreSQL backup and recovery system using a Raspberry Pi. The system uses pg_dump to create timestamped database backups from the production database, stores credentials securely using PostgreSQL's .pgpass file, schedules nightly backups using cron, and includes retention logic to remove backups older than 30 days. A restore test was performed by recreating the backup database schema, restoring a production backup file, and verifying restored tables and data.
