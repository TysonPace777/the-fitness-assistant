
#!/bin/bash

# Create timestamp
DATE=$(date +"%Y-%m-%d_%H-%M")

# Backup location
BACKUP_DIR="/home/holly/db_backups"

# Filename
BACKUP_FILE="$BACKUP_DIR/fitness_backup_$DATE.sql"

# Database connection information
HOST="dpg-d9cli261a83c739b3tkg-a.oregon-postgres.render.com"
USER="the_fitness_assistant_user"
DATABASE="the_fitness_assistant"

# Create backup
/usr/lib/postgresql/18/bin/pg_dump \
--no-owner \
-h "$HOST" \
-U "$USER" \
-d "$DATABASE" \
-f "$BACKUP_FILE"

# Check success
if [ $? -eq 0 ]; then
    echo "Backup completed: $BACKUP_FILE"

    # Delete backups older than 30 days
    find "$BACKUP_DIR" \
    -name "fitness_backup_*.sql" \
    -mtime +30 \
    -delete

    echo "Old backups cleaned up."

else
    echo "Backup failed!"
    rm -f "$BACKUP_FILE"
fi
