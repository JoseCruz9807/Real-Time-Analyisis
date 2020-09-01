CREATE TRIGGER insertsMeetingBegins ON dbo.MEETING_BEGINS INSTEAD OF INSERT  AS
BEGIN
DECLARE @correoIn VARCHAR(255)
DECLARE @uuidIn VARCHAR(255)
DECLARE @correolikeIn VARCHAR(255)
DECLARE @horainicioIn DATETIME
SELECT @correoIn=CORREO, @uuidIn=UUID,@correolikeIn=CORREOLIKE,@horainicioIn=HORAINICIO FROM inserted
EXEC insertarMeeting @correo=@correoIn,@uuid=@uuidIn, @correolike=@correolikeIn, @horainicio=@horainicioIn
END
GO


CREATE TRIGGER insertsSA ON dbo.STREAM_LANDING_TABLE INSTEAD OF INSERT  AS
BEGIN
DECLARE @correoIn VARCHAR(255)
DECLARE @uuidIn VARCHAR(255)
DECLARE @correolikeIn VARCHAR(255)
DECLARE @horainicioIn DATETIME
DECLARE @eventType VARCHAR(255)
DECLARE @joinUrlIn VARCHAR(255)
SELECT @correoIn=CORREO, @uuidIn=UUID,@correolikeIn=CORREOLIKE,@horainicioIn=HORAINICIO, @eventType=EVENTTYPE, @joinUrlIn=JOINURL FROM inserted
IF @eventType='MeetingStarts'
BEGIN
    EXEC insertarMeeting @correo=@correoIn,@uuid=@uuidIn, @correolike=@correolikeIn, @horainicio=@horainicioIn, @joinurl=@joinUrlIn
END 
ELSE
BEGIN
    IF @eventType='ParticipantJoin'
    BEGIN
        EXEC agregarParticipante @uuid=@uuidIn
    END 
    ELSE
    BEGIN
        IF @eventType='ParticipantLeft'
        BEGIN
            EXEC quitarParticipante @uuid=@uuidIn
        END 
        ELSE
        BEGIN
            EXEC borrarMeeting @uuid=@uuidIn
        END
    END
END
END
GO


CREATE TRIGGER insertsMeetingEnds ON dbo.MEETING_ENDS INSTEAD OF INSERT  AS
DECLARE @uuidIn VARCHAR(255)
SELECT @uuidIn=UUID FROM inserted
EXEC borrarMeeting @uuid=@uuidIn
GO



CREATE TRIGGER insertsParticipantJoin ON dbo.PARTICIPANT_JOIN INSTEAD OF INSERT  AS
DECLARE @uuidIn VARCHAR(255)
SELECT @uuidIn=UUID FROM inserted
EXEC agregarParticipante @uuid=@uuidIn
GO


CREATE TRIGGER insertsParticipantLeft ON dbo.PARTICIPANT_LEFT INSTEAD OF INSERT  AS
DECLARE @uuidIn VARCHAR(255)
SELECT @uuidIn=UUID FROM inserted
EXEC quitarParticipante @uuid=@uuidIn
GO

