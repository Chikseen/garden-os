CREATE TABLE IF NOT EXISTS GARDEN (
    ID CHARACTER VARYING(36) PRIMARY KEY,
    NAME VARCHAR(255) NOT NULL
);

CREATE TABLE IF NOT EXISTS USERS (
    ID CHARACTER VARYING(36) PRIMARY KEY,
    NAME VARCHAR(255) NOT NULL,
    API_KEY VARCHAR(128) NOT NULL,
    GARDEN_ID CHARACTER VARYING(36),
    CONSTRAINT FK_GARDEN FOREIGN KEY(GARDEN_ID) REFERENCES GARDEN(ID)
);

CREATE TABLE IF NOT EXISTS MAPS (
    ID CHARACTER VARYING(36) PRIMARY KEY,
    NAME VARCHAR(255) NOT NULL,
    GARDEN_ID CHARACTER VARYING(36) NOT NULL,
    JSON JSON,
    CONSTRAINT FK_GARDEN FOREIGN KEY(GARDEN_ID) REFERENCES GARDEN(ID) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS DEVICES (
    ID CHARACTER VARYING(36) NOT NULL PRIMARY KEY,
    NAME CHARACTER VARYING(255) NOT NULL,
    UPPER_LIMIT SMALLINT NOT NULL DEFAULT 100,
    LOWER_LIMIT SMALLINT NOT NULL DEFAULT 0,
    DEVICE_TYP CHARACTER VARYING(255) NOT NULL,
    ADDRESS CHARACTER VARYING(255) NOT NULL,
    SERIAL_ID SMALLINT DEFAULT 0,
    DISPLAY_ID CHARACTER VARYING(255),
    DATA_UPDATE_INTERVAL TIME WITHOUT TIME ZONE NOT NULL DEFAULT '00:00:10',
    GARDEN_ID CHARACTER VARYING(36) NOT NULL,
    CONSTRAINT FK_GARDEN FOREIGN KEY(GARDEN_ID) REFERENCES GARDEN(ID) ON DELETE CASCADE,
    CHECK (UPPER_LIMIT BETWEEN LOWER_LIMIT AND 100),
    CHECK (LOWER_LIMIT BETWEEN 0 AND UPPER_LIMIT)
);

CREATE TABLE IF NOT EXISTS DATALOG (
    ID CHARACTER VARYING(36) PRIMARY KEY,
    VALUE SMALLINT NOT NULL,
    DATE TIMESTAMP WITHOUT TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    DEVICE_ID CHARACTER VARYING(36) NOT NULL,
    CONSTRAINT FK_DEVICE FOREIGN KEY(DEVICE_ID) REFERENCES DEVICES(ID) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS RPIS (
    ID CHARACTER VARYING(36) PRIMARY KEY,
    GARDEN_ID CHARACTER VARYING(36) NOT NULL,
    CONSTRAINT FK_GARDEN FOREIGN KEY(GARDEN_ID) REFERENCES GARDEN(ID) ON DELETE CASCADE,
);

--New User
INSERT INTO USERS (
    ID,
    NAME,
    API_KEY
) VALUES (
    GEN_RANDOM_UUID(),
    'TestUser',
    'test'
);

--New Garden
INSERT INTO GARDEN (
    ID,
    NAME
) VALUES (
    GEN_RANDOM_UUID(),
    'TestGarden'
);

--New Device
INSERT INTO DEVICES (
    ID,
    NAME,
    DEVICE_TYP,
    ADDRESS,
    GARDEN_ID
) VALUES (
    GEN_RANDOM_UUID(),
    'TestPotiONE',
    'i2c_adc_7080',
    '0xcc',
    'accd30d2-7392-40b7-8a08-6d9ac9cc22b6'
);

INSERT INTO RPIS (
    ID,
    GARDEN_ID
) VALUES (
    GEN_RANDOM_UUID(),
    'd1526afc-9eba-4ee6-b933-c2bcd6c6ef92'
);

INSERT INTO DEVICES (
    ID,
    NAME,
    DEVICE_TYP,
    ADDRESS,
    GARDEN_ID
) VALUES (
    GEN_RANDOM_UUID(),
    'TestPotiTWO',
    'i2c_adc_7080',
    '0x8c',
    'accd30d2-7392-40b7-8a08-6d9ac9cc22b6'
);

INSERT INTO MAPS (
    ID,
    NAME,
    JSON,
    GARDEN_ID
) VALUES (
    GEN_RANDOM_UUID(),
    'TestGarden_Base_Layer',
    '{"type":"FeatureCollection","features":[{"type":"Feature","geometry":{"type":"Polygon","coordinates":[[[-37.5,-48],[-5,-48],[0,-45],[5,-45],[10,-48],[37.5,-48],[37.5,49],[-37.5,49],[-37.5,-48]],[[0,0.5],[0,0.5]]]},"properties":{"id":"main","name":"Garten","color":"#ceead6","area":491.4}},{"type":"Feature","geometry":{"type":"Polygon","coordinates":[[[-27,38.51],[6.5,38.51],[6.5,29.509999999999998],[-27,29.509999999999998],[-27,38.51]]]},"properties":{"id":"veranda","name":"Veranda","color":"#fef9e8","area":16.08}},{"type":"Feature","geometry":{"type":"Polygon","coordinates":[[[-37.5,27.02],[0,27.02],[0,16.02],[-37.5,16.02],[-37.5,27.02]]]},"properties":{"id":"flowerbed_veranda","name":"Blumenbeet","color":"#aeddba","area":24}},{"type":"Feature","geometry":{"type":"Polygon","coordinates":[[[-35.5,47.51],[6.5,47.51],[6.5,38.51],[-35.5,38.51],[-35.5,47.51]]]},"properties":{"id":"house","name":"Hütte","color":"#f2f4f5","area":20.16}},{"type":"Feature","geometry":{"type":"Polygon","coordinates":[[[17.5,42.5],[32.5,42.5],[32.5,31.5],[17.5,31.5],[17.5,42.5]]]},"properties":{"id":"green_house","name":"Gewächshaus","color":"#cfd1d3","area":9}},{"type":"Feature","geometry":{"type":"Polygon","coordinates":[[[-37.5,16.009999999999998],[0,16.009999999999998],[0,1.009999999999998],[-37.5,1.009999999999998],[-37.5,16.009999999999998]]]},"properties":{"id":"outside_area","name":"Sitzbereich","color":"#fef9e8","area":33}},{"type":"Feature","geometry":{"type":"Polygon","coordinates":[[[5.5,-8.980000000000004],[37.5,-8.980000000000004],[37.5,-20.980000000000004],[5.5,-20.980000000000004],[5.5,-8.980000000000004]]]},"properties":{"id":"producebed_1","name":"Nutzplfanzenbeet","color":"#f2ead8","area":25.6}},{"type":"Feature","geometry":{"type":"Polygon","coordinates":[[[5.5,-22],[37.5,-22],[37.5,-31],[5.5,-31],[5.5,-22]]]},"properties":{"id":"producebed_2","name":"Nutzplfanzenbeet","color":"#f2ead8","area":16}},{"type":"Feature","geometry":{"type":"Polygon","coordinates":[[[5.5,-31.989999999999995],[37.5,-31.989999999999995],[37.5,-43.989999999999995],[5.5,-43.989999999999995],[5.5,-31.989999999999995]]]},"properties":{"id":"producebed_3","name":"Nutzplfanzenbeet","color":"#f2ead8","area":25.6}},{"type":"Feature","geometry":{"type":"Polygon","coordinates":[[[-37.5,-44.980000000000004],[0,-44.980000000000004],[-5,-47.980000000000004],[-37.5,-47.980000000000004],[-37.5,-44.980000000000004]]]},"properties":{"id":"flowerbed_entry_left","name":"Blumenbeet","color":"#aeddba","area":3.5}},{"type":"Feature","geometry":{"type":"Polygon","coordinates":[[[4.5,-44.980000000000004],[37.5,-44.980000000000004],[37.5,-47.980000000000004],[9.5,-47.980000000000004],[4.5,-44.980000000000004]]]},"properties":{"id":"flowerbed_entry_right","name":"Blumenbeet","color":"#aeddba","area":3.05}},{"type":"Feature","geometry":{"type":"Polygon","coordinates":[[[-37.5,-38.980000000000004],[-7.5,-38.980000000000004],[-7.5,1.019999999999996],[0,1.019999999999996],[0,-43.980000000000004],[-37.5,-43.980000000000004],[-37.5,-38.980000000000004]]]},"properties":{"id":"flowerbed_left_long","name":"Blumenbeet","color":"#aeddba","area":27.75}}]}',
    'accd30d2-7392-40b7-8a08-6d9ac9cc22b6'
);

INSERT INTO MAPS (
    ID,
    NAME,
    JSON,
    GARDEN_ID
) VALUES (
    GEN_RANDOM_UUID(),
    'TestGarden_Detailed_Layer',
    '{"type":"FeatureCollection","features":[{"type":"Feature","geometry":{"type":"Polygon","coordinates":[[[-35.5,39.51],[-30.5,39.51],[-30.5,46.51],[-35.5,46.51],[-35.5,39.51]]]},"properties":{"id":"house_couch","name":"Couch","color":"#ffffff","area":3}},{"type":"Feature","geometry":{"type":"Polygon","coordinates":[[[-16.5,41.489999999999995],[-11.5,41.489999999999995],[-11.5,47.489999999999995],[-16.5,47.489999999999995],[-16.5,41.489999999999995]]]},"properties":{"id":"house_TV","name":"TV","color":"#ffffff","area":2}},{"type":"Feature","geometry":{"type":"Polygon","coordinates":[[[-11.5,43.491],[6.5,43.491],[6.5,47.491],[-11.5,47.491],[-11.5,43.491]]]},"properties":{"id":"house_kitchen","name":"Kitchen","color":"#ffffff","area":5.4}}]}',
    'accd30d2-7392-40b7-8a08-6d9ac9cc22b6'
);

------------------------------------------------------------------------------
-- Get rpi data --

SELECT
    RPIS.ID   AS RPI_ID,
    GARDEN.ID AS GARDEN_ID,
    GARDEN.NAME
FROM
    RPIS
    JOIN GARDEN
    ON RPIS.ID = '{id}'
    AND RPIS.API_KEY = '{ApiKey}'
    AND RPIS.GARDEN_ID = GARDEN.ID;

-- Get rpi Devices --
SELECT
    DEVICES.NAME AS DEVICE_NAME,
    DEVICES.LOWER_LIMIT,
    DEVICES.UPPER_LIMIT,
    DEVICES.DEVICE_TYP,
    DEVICES.ADDRESS,
    DEVICES.SERIAL_ID,
    DEVICES.DISPLAY_ID,
    DEVICES.DATA_UPDATE_INTERVAL
FROM
    RPIS
    JOIN GARDEN
    ON RPIS.ID = '{id}'
    AND RPIS.API_KEY = '{ApiKey}'
    AND RPIS.GARDEN_ID = GARDEN.ID JOIN DEVICES
    ON RPIS.GARDEN_ID = DEVICES.GARDEN_ID;