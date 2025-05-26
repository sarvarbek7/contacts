START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM ef_migrations WHERE "migration_id" = '20250526095256_AddTypeToPhoneNumber') THEN
    ALTER TABLE phone_number ADD type integer NOT NULL DEFAULT 0;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM ef_migrations WHERE "migration_id" = '20250526095256_AddTypeToPhoneNumber') THEN
    INSERT INTO ef_migrations (migration_id, product_version)
    VALUES ('20250526095256_AddTypeToPhoneNumber', '9.0.4');
    END IF;
END $EF$;
COMMIT;

