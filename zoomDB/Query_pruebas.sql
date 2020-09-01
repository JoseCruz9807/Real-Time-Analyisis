SELECT TOP (1000) [HORAINICIOSESIONZOOM]
      ,[HORAFINCLASE]
      ,[START_TIME]
      ,[PARTICIPANTESACTIVOS]
      ,[CRN]
      ,[CLAVEEJERCICIOACADEMICO]
      ,[CORREOELECTRONICOPROFESOR]
      ,[UUID]
      ,[JOINURL]
  FROM [dbo].[LIVE_MEETINGS]

  
EXEC updateColumnasDWH
SELECT * FROM dbo.LIVE_MEETINGS
  WHERE HORAFINCLASE IS NOT NULL AND PARTICIPANTESACTIVOS>0

DELETE FROM  dbo.SIMULADOR_DWH 
SELECT * FROM dbo.LIVE_MEETINGS 
WHERE CRN IS NULL AND HORAINICIOSESIONZOOM>'07/29/2020 13:30:00'
WHERE  HORAINICIOSESIONZOOM='07/29/2020 14:00:00'
WHERE CORREOELECTRONICOPROFESOR='jose_vicent@tec.mx'
WHERE HORAINICIOSESIONZOOM='07/29/2020 9:30:00' 
AND HORAINICIOSESIONZOOM<'07/30/2020 8:59:00'
    WHERE CORREOELECTRONICOPROFESOR LIKE 'pablo.renteria@%' AND HORAINICIOSESIONZOOM<>840 AND UUID IS NOT NULL
 
SELECT  * FROM dbo.STREAM_LANDING_TABLE
WHERE CORREOLIKE = 'eb.trenado@%'
SELECT count(*) FROM dbo.SIMULADOR_DWH
DELETE FROM dbo.LIVE_MEETINGS
WHERE HORAFINCLASE IS NULL

INSERT INTO dbo.STREAM_LANDING_TABLE (CORREO, 
    UUID, 
    CORREOLIKE, 
    HORAINICIO,
    EVENTTYPE,
    JOINURL)
VALUES ('jose.cruz@gmail.com', '1234', 'jose.cruz%', '10/10/2020 8:44:00', 'ParticipantJoin','prueba.com')



EXEC insertarMeeting @correo='josecruz98@gmail.com', @uuid='123', @correolike='josecruz98%', @horainicio='10/10/2020 8:44:00'



  EXEC insertarClase @inicio_zoom=840, @fin_clase=900, @inicio_meeting='07/25/2020 8:44:00', @crn=1, @term=2, @correo='josecruz@gmail.com', @correolike='josecruz%'




SELECT
    U.email as CORREO, 
    I.payload.object.uuid as UUID, 
    CONCAT(SUBSTRING(U.email,0,CHARINDEX('@',U.email)),'%')  as CORREOLIKE, 
    UDF.getDate(2)  as HORAINICIO
INTO
    outputStart
FROM 
    inputEventHub I
JOIN
    inputUsuarios U
ON I.payload.object.host_id = U.id
WHERE I.event='meeting.started'

SELECT 
    I.payload.object.uuid as UUID
INTO
    outputJoin
FROM 
    inputEventHub I
JOIN
    inputUsuarios U
ON I.payload.object.host_id = U.id
WHERE I.event='meeting.participant_joined'

SELECT 
    I.payload.object.uuid as UUID
INTO
    outputLeft
FROM 
    inputEventHub I
JOIN
    inputUsuarios U
ON I.payload.object.host_id = U.id
WHERE I.event='meeting.participant_left'

SELECT 
    I.payload.object.uuid as UUID
INTO
    outputEnd
FROM 
    inputEventHub I
JOIN
    inputUsuarios U
ON I.payload.object.host_id = U.id
WHERE I.event='meeting.ended'

