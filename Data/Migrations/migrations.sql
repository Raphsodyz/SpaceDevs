SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;
SET default_tablespace = '';
SET default_table_access_method = heap;

CREATE EXTENSION IF NOT EXISTS pg_trgm WITH SCHEMA public;
COMMENT ON EXTENSION pg_trgm IS 'Text similarity measurement and index searching based on trigrams.';

CREATE TABLE IF NOT EXISTS public.orbit(
    id UUID PRIMARY KEY,
    id_from_api INT NULL,
    atualization_date TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    imported_t TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    status VARCHAR(15) NOT NULL,
    name VARCHAR(360) NULL,
    abbrev VARCHAR(360) NULL
);

CREATE TABLE IF NOT EXISTS public.mission(
    id UUID PRIMARY KEY,
    id_from_api INT NULL,
    atualization_date TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    imported_t TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    status VARCHAR(15) NOT NULL,
    launch_library_id INT NULL,
    name VARCHAR(360) NULL,
    description VARCHAR(2500) NULL,
    type VARCHAR(360) NULL,
    id_orbit UUID NULL,
    launch_designator VARCHAR(360) NULL,
    search VARCHAR(360) GENERATED ALWAYS AS (
        LOWER(name)
    ) STORED NULL,

    CONSTRAINT fk_mission_orbit FOREIGN KEY (id_orbit) REFERENCES public.orbit(id)
);

CREATE TABLE IF NOT EXISTS public.configuration(
    id UUID PRIMARY KEY,
    id_from_api INT NULL,
    atualization_date TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    imported_t TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    status VARCHAR(15) NOT NULL,
    launch_library_id INT NULL,
    url VARCHAR(1000) NULL,
    name VARCHAR(360) NULL,
    family VARCHAR(360) NULL,
    full_name VARCHAR(360) NULL,
    variant VARCHAR(360) NULL,
    search VARCHAR(600) GENERATED ALWAYS AS (
        LOWER(name || ' ' || family)
    ) STORED NULL
);

CREATE TABLE IF NOT EXISTS public.status(
    id UUID PRIMARY KEY,
    id_from_api INT NULL,
    atualization_date TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    imported_t TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    status VARCHAR(15) NOT NULL,
    name VARCHAR(360) NULL,
    abbrev VARCHAR(360) NULL,
    description VARCHAR(2500) NULL
);

CREATE TABLE IF NOT EXISTS public.launch_service_provider(
    id UUID PRIMARY KEY,
    id_from_api INT NULL,
    atualization_date TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    imported_t TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    status VARCHAR(15) NOT NULL,    
    url VARCHAR(1000) NULL,
    name VARCHAR(360) NULL,
    type VARCHAR(360) NULL
);

CREATE TABLE IF NOT EXISTS public.location(
    id UUID PRIMARY KEY,
    id_from_api INT NULL,
    atualization_date TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    imported_t TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    status VARCHAR(15) NOT NULL,
    url VARCHAR(1000) NULL,
    name VARCHAR(360) NULL,
    country_code VARCHAR(360) NULL,
    map_image VARCHAR(1000) NULL,
    total_launch_count INT NULL,
    total_landing_count INT NULL,
    search VARCHAR(360) GENERATED ALWAYS AS (
        LOWER(name)
    ) STORED NULL
);

CREATE TABLE IF NOT EXISTS public.pad(
    id UUID PRIMARY KEY,
    id_from_api INT NULL,
    atualization_date TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    imported_t TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    status VARCHAR(15) NOT NULL,
    url VARCHAR(1000) NULL,
    agency_id INT NULL,
    name VARCHAR(360) NULL,
    info_url VARCHAR(1000) NULL,
    wiki_url VARCHAR(1000) NULL,
    map_url VARCHAR(1000) NULL,
    latitude DOUBLE PRECISION NULL,
    longitude DOUBLE PRECISION NULL,
    id_location UUID NULL,
    map_image VARCHAR(1000) NULL,
    total_launch_count INT NULL,
    search VARCHAR(360) GENERATED ALWAYS AS (
        LOWER(name)
    ) STORED NULL,

    CONSTRAINT fk_pad_location FOREIGN KEY (id_location) REFERENCES public.location(id)
);

CREATE TABLE IF NOT EXISTS public.rocket(
    id UUID PRIMARY KEY,
    id_from_api INT NULL,
    atualization_date TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    imported_t TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    status VARCHAR(15) NOT NULL,
    id_configuration UUID NULL,

    CONSTRAINT fk_rocket_configuration FOREIGN KEY(id_configuration) REFERENCES public.configuration(id)
);

CREATE TABLE IF NOT EXISTS public.launch(
    id UUID PRIMARY KEY,
    id_from_api INT NULL,
    atualization_date TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    imported_t TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    status VARCHAR(15) NOT NULL,
    api_guid UUID NOT NULL,
    url VARCHAR(1000) NOT NULL,
    launch_library_id INT NULL,
    slug VARCHAR(360) NULL,
    name VARCHAR(360) NULL,
    id_status UUID NULL,
    net TIMESTAMP WITHOUT TIME ZONE NULL,
    window_end TIMESTAMP WITHOUT TIME ZONE NULL,
    window_start TIMESTAMP WITHOUT TIME ZONE NULL,
    inhold BOOLEAN NULL,
    tbd_time BOOLEAN NULL,
    tbd_date BOOLEAN NULL,
    probability INT NULL,
    hold_reason VARCHAR(360) NULL,
    fail_reason VARCHAR(360) NULL,
    hashtag VARCHAR(360) NULL,
    id_launch_service_provider UUID NULL,
    id_rocket UUID NULL,
    id_mission UUID NULL,
    id_pad UUID NULL,
    web_cast_live BOOLEAN NULL,
    image VARCHAR(1000) NULL,
    infographic VARCHAR(360) NULL,
    programs VARCHAR(360) NULL,
    search VARCHAR(600) GENERATED ALWAYS AS (
        LOWER(name || ' ' || slug)
    ) STORED NULL,

    CONSTRAINT fk_launch_status FOREIGN KEY (id_status) REFERENCES public.status(id),
    CONSTRAINT fk_launch_launch_service_provider FOREIGN KEY (id_launch_service_provider) REFERENCES public.launch_service_provider(id),
    CONSTRAINT fk_launch_rocket FOREIGN KEY (id_rocket) REFERENCES public.rocket(id),
    CONSTRAINT fk_launch_mission FOREIGN KEY (id_mission) REFERENCES public.mission(id),
    CONSTRAINT fk_launch_pad FOREIGN KEY (id_pad) REFERENCES public.pad(id)
);

CREATE TABLE IF NOT EXISTS public.update_log_routine(
    id UUID PRIMARY KEY,
    id_from_api INT NULL,
    atualization_date TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    imported_t TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    status VARCHAR(15) NOT NULL,
    transaction_date TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    offset_data INT NOT NULL,
    success BOOLEAN NOT NULL,
    message VARCHAR(360) NOT NULL,
    entity_count INT NOT NULL,
    origin VARCHAR(360) NOT NULL
);

CREATE MATERIALIZED VIEW IF NOT EXISTS public.launch_view AS
    SELECT
        l.id AS launch_id,
        l.atualization_date AS launch_atualization_date,
        l.imported_t AS launch_imported_t,
        l.status AS launch_status,
        l.api_guid AS launch_api_guid,
        l.url AS launch_url,
        l.launch_library_id AS launch_launch_library_id,
        l.slug AS launch_slug,
        l.name AS launch_name,
        l.id_status, l.net AS launch_net,
        l.window_end AS launch_window_end,
        l.window_start AS launch_window_start,
        l.inhold AS launch_inhold,
        l.tbd_time AS launch_tbd_time,
        l.tbd_date AS launch_tbd_date,
        l.probability AS launch_probability,
        l.hold_reason AS launch_hold_reason,
        l.fail_reason AS launch_fail_reason,
        l.hashtag AS launch_hashtag,
        l.id_launch_service_provider,
        l.id_rocket,
        l.id_mission,
        l.id_pad,
        l.web_cast_live AS launch_web_cast_live,
        l.image AS launch_image,
        l.infographic AS launch_infographic,
        l.programs AS launch_programs,
        s.name AS status_name,
        s.abbrev AS status_abbrev,
        s.description AS status_description,
        lsp.url AS launch_service_provider_url,
        lsp.name AS launch_service_provider_name,
        lsp.type AS launch_service_provider_type,
        r.id_configuration,
        c.launch_library_id AS configuration_launch_library_id,
        c.url AS configuration_url,
        c.name AS configuration_name,
        c.family AS configuration_family,
        c.full_name AS configuration_full_name,
        c.variant AS configuration_variant,
        m.launch_library_id AS mission_launch_library_id, 
        m.name AS mission_name,
        m.description AS mission_description,
        m.type AS mission_type,
        m.id_orbit,
        m.launch_designator AS mission_launch_designator,
        o.name AS orbit_name,
        o.abbrev AS orbit_abbrev, 
        p.url AS pad_url,
        p.agency_id AS pad_agency_id,
        p.name AS pad_name,
        p.info_url AS pad_info_url,
        p.wiki_url AS pad_wiki_url,
        p.map_url AS pad_map_url,
        p.latitude AS pad_latitude,
        p.longitude AS pad_longitude,
        p.id_location,
        p.map_image AS pad_map_image,
        p.total_launch_count AS pad_total_launch_count,
        loc.url AS location_url,
        loc.name AS location_name,
        loc.country_code AS location_country_code,
        loc.map_image AS location_map_image,
        loc.total_launch_count AS location_total_launch_count,
        loc.total_landing_count AS location_total_landing_count
    FROM
        public.launch AS l
        LEFT JOIN public.status AS s ON l.id_status = s.id
        LEFT JOIN public.launch_service_provider AS lsp ON l.id_launch_service_provider = lsp.id
        LEFT JOIN public.rocket AS r ON l.id_rocket = r.id
        LEFT JOIN public.configuration AS c ON r.id_configuration = c.id
        LEFT JOIN public.mission AS m ON l.id_mission = m.id
        LEFT JOIN public.orbit AS o ON m.id_orbit = o.id
        LEFT JOIN public.pad AS p ON l.id_pad = p.id
        LEFT JOIN public.location AS loc ON p.id_location = loc.id
    WHERE
        l.status = 'PUBLISHED';

CREATE INDEX IDX_GIST_LAUNCH_SLUG_NAME ON public.launch USING gist (search public.gist_trgm_ops);
CREATE INDEX IDX_GIST_CONFIGURATION_NAME_FAMILY ON public.configuration USING gist (search public.gist_trgm_ops);
CREATE INDEX IDX_GIST_MISSION_NAME ON public.mission USING gist (search public.gist_trgm_ops);
CREATE INDEX IDX_GIST_LOCATION_NAME ON public.location USING gist (search public.gist_trgm_ops);
CREATE INDEX IDX_GIST_PAD_NAME ON public.pad USING gist (search public.gist_trgm_ops);

CREATE INDEX idx_mission_id_orbit ON public.mission USING btree(id_orbit);
CREATE INDEX idx_pad_id_location ON public.pad USING btree(id_location);
CREATE INDEX idx_rocket_id_configuration ON public.rocket USING btree(id_configuration);
CREATE INDEX idx_launch_id_status ON public.launch USING btree(id_status);
CREATE INDEX idx_launch_id_launch_service_provider ON public.launch USING btree(id_launch_service_provider);
CREATE INDEX idx_launch_id_rocket ON public.launch USING btree(id_rocket);
CREATE INDEX idx_launch_id_mission ON public.launch USING btree(id_mission);
CREATE INDEX idx_launch_id_pad ON public.launch USING btree(id_pad);

SET search_path TO "$user", public;