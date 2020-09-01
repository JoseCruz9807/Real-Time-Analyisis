SELECT 
    CONCAT(SUBSTRING(U.email,0,CHARINDEX('@',U.email)),'@') as CORREO, 
    I.payload.object.host_id as HOSTID, 
    I.payload.object.uuid as UUID, 
    UDF.getDate(2)  as HORAINICIO,
    I.event as EVENTTYPE
INTO
    logsSA
FROM 
    inputEventHub I
JOIN
    inputUsuarios U
ON I.payload.object.host_id = U.id
JOIN
    inputClases C
ON CONCAT(SUBSTRING(U.email,0,CHARINDEX('@',U.email)),'@') = C.correoelectronicoprofesor

SELECT
    U.email as CORREO, 
    I.payload.object.host_id as UUID, 
    CONCAT(SUBSTRING(U.email,0,CHARINDEX('@',U.email)),'@%')  as CORREOLIKE, 
    UDF.getDate(2)  as HORAINICIO,
    'MeetingStarts' as EVENTTYPE,
    U.personal_meeting_url AS JOINURL
INTO
    outputDb
FROM 
    inputEventHub I
JOIN
    inputUsuarios U
ON I.payload.object.host_id = U.id
JOIN
    inputClases C
ON CONCAT(SUBSTRING(U.email,0,CHARINDEX('@',U.email)),'@') = C.correoelectronicoprofesor
WHERE I.event='meeting.started'

UNION

SELECT 
    NULL as CORREO,
    I.payload.object.host_id as UUID,
    NULL AS CORREOLIKE,
    NULL AS HORAINICIO,
    'ParticipantJoin' as EVENTTYPE,
    NULL AS JOINURL
FROM 
    inputEventHub I
JOIN
    inputUsuarios U
ON I.payload.object.host_id = U.id
JOIN
    inputClases C
ON CONCAT(SUBSTRING(U.email,0,CHARINDEX('@',U.email)),'@') = C.correoelectronicoprofesor
WHERE I.event='meeting.participant_joined'

UNION

SELECT 
    NULL as CORREO,
    I.payload.object.host_id as UUID,
    NULL AS CORREOLIKE,
    NULL AS HORAINICIO,
    'ParticipantLeft' as EVENTTYPE,
    NULL AS JOINURL
FROM 
    inputEventHub I
JOIN
    inputUsuarios U
ON I.payload.object.host_id = U.id
JOIN
    inputClases C
ON CONCAT(SUBSTRING(U.email,0,CHARINDEX('@',U.email)),'@') = C.correoelectronicoprofesor
WHERE I.event='meeting.participant_left'

UNION

SELECT 
    NULL as CORREO,
    I.payload.object.host_id as UUID,
    NULL AS CORREOLIKE,
    NULL AS HORAINICIO,
    'MeetingEnds' as EVENTTYPE,
    NULL AS JOINURL
FROM 
    inputEventHub I
JOIN
    inputUsuarios U
ON I.payload.object.host_id = U.id
JOIN
    inputClases C
ON CONCAT(SUBSTRING(U.email,0,CHARINDEX('@',U.email)),'@') = C.correoelectronicoprofesor
WHERE I.event='meeting.ended'

