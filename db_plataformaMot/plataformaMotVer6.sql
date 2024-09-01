USE [master]
GO
/****** Object:  Database [plataformaMotVer6]    Script Date: 24/08/2024 7:11:26 p. m. ******/
CREATE DATABASE [plataformaMotVer6]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'plataformaMotVer6', FILENAME = N'C:\SQLData\plataformaMotVer6.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'plataformaMotVer6_log', FILENAME = N'C:\SQLData\plataformaMotVer6_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [plataformaMotVer6].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [plataformaMotVer6] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [plataformaMotVer6] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [plataformaMotVer6] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [plataformaMotVer6] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [plataformaMotVer6] SET ARITHABORT OFF 
GO
ALTER DATABASE [plataformaMotVer6] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [plataformaMotVer6] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [plataformaMotVer6] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [plataformaMotVer6] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [plataformaMotVer6] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [plataformaMotVer6] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [plataformaMotVer6] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [plataformaMotVer6] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [plataformaMotVer6] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [plataformaMotVer6] SET  DISABLE_BROKER 
GO
ALTER DATABASE [plataformaMotVer6] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [plataformaMotVer6] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [plataformaMotVer6] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [plataformaMotVer6] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [plataformaMotVer6] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [plataformaMotVer6] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [plataformaMotVer6] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [plataformaMotVer6] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [plataformaMotVer6] SET  MULTI_USER 
GO
ALTER DATABASE [plataformaMotVer6] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [plataformaMotVer6] SET DB_CHAINING OFF 
GO
ALTER DATABASE [plataformaMotVer6] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [plataformaMotVer6] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
USE [plataformaMotVer6]
GO
/****** Object:  Table [dbo].[tblActividades]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblActividades](
	[idActividad] [int] IDENTITY(1,1) NOT NULL,
	[nombreActividad] [varchar](200) NOT NULL,
	[categoria] [varchar](20) NOT NULL,
	[horasAsignadas] [int] NULL,
	[descripcion] [varchar](500) NOT NULL,
	[fechaInicio] [date] NOT NULL,
	[fechaFinalizacion] [date] NULL,
	[tipoDocumentoBienestar] [varchar](10) NULL,
	[numeroDocumentoBienestar] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[idActividad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblAprendices]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblAprendices](
	[tipoDocumentoAprendiz] [varchar](10) NOT NULL,
	[numeroDocumentoAprendiz] [varchar](20) NOT NULL,
	[totalHorasAsistidas] [int] NULL,
	[copiaFichaNumero] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[tipoDocumentoAprendiz] ASC,
	[numeroDocumentoAprendiz] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblBienestar]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblBienestar](
	[tipoDocumentoBienestar] [varchar](10) NOT NULL,
	[numeroDocumentoBienestar] [varchar](20) NOT NULL,
	[cargo] [varchar](30) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[tipoDocumentoBienestar] ASC,
	[numeroDocumentoBienestar] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblFichas]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblFichas](
	[fichaNumero] [varchar](20) NOT NULL,
	[nombrePrograma] [varchar](100) NOT NULL,
	[fechaInicio] [date] NOT NULL,
	[fechaFinalizacion] [date] NULL,
	[jornada] [varchar](10) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[fichaNumero] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblRegistroAsistencias]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblRegistroAsistencias](
	[idAsistencia] [int] IDENTITY(1,1) NOT NULL,
	[estadoValidacionBienestar] [bit] NULL,
	[fechaValidacion] [date] NULL,
	[horasAsistidas] [int] NULL,
	[copiaIdActividad] [int] NOT NULL,
	[tipoDocumentoAprendiz] [varchar](10) NOT NULL,
	[numeroDocumentoAprendiz] [varchar](20) NOT NULL,
	[tipoDocumentoBienestar] [varchar](10) NULL,
	[numeroDocumentoBienestar] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[idAsistencia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblUsuarios]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblUsuarios](
	[tipoDocumento] [varchar](10) NOT NULL,
	[numeroDocumento] [varchar](20) NOT NULL,
	[nombres] [varchar](80) NOT NULL,
	[apellidos] [varchar](80) NOT NULL,
	[correo] [varchar](100) NOT NULL,
	[clave] [varchar](250) NOT NULL,
	[rol] [varchar](20) NOT NULL,
	[token_recuperacion] [varchar](250) NULL,
	[fecha_expiracion_token] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[tipoDocumento] ASC,
	[numeroDocumento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tblActividades] ADD  DEFAULT ((0)) FOR [horasAsignadas]
GO
ALTER TABLE [dbo].[tblAprendices] ADD  DEFAULT ((0)) FOR [totalHorasAsistidas]
GO
ALTER TABLE [dbo].[tblRegistroAsistencias] ADD  DEFAULT ((0)) FOR [estadoValidacionBienestar]
GO
ALTER TABLE [dbo].[tblRegistroAsistencias] ADD  DEFAULT (getdate()) FOR [fechaValidacion]
GO
ALTER TABLE [dbo].[tblRegistroAsistencias] ADD  DEFAULT ((0)) FOR [horasAsistidas]
GO
ALTER TABLE [dbo].[tblActividades]  WITH CHECK ADD FOREIGN KEY([tipoDocumentoBienestar], [numeroDocumentoBienestar])
REFERENCES [dbo].[tblBienestar] ([tipoDocumentoBienestar], [numeroDocumentoBienestar])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tblAprendices]  WITH CHECK ADD FOREIGN KEY([copiaFichaNumero])
REFERENCES [dbo].[tblFichas] ([fichaNumero])
GO
ALTER TABLE [dbo].[tblAprendices]  WITH CHECK ADD FOREIGN KEY([tipoDocumentoAprendiz], [numeroDocumentoAprendiz])
REFERENCES [dbo].[tblUsuarios] ([tipoDocumento], [numeroDocumento])
GO
ALTER TABLE [dbo].[tblBienestar]  WITH CHECK ADD FOREIGN KEY([tipoDocumentoBienestar], [numeroDocumentoBienestar])
REFERENCES [dbo].[tblUsuarios] ([tipoDocumento], [numeroDocumento])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tblRegistroAsistencias]  WITH CHECK ADD FOREIGN KEY([copiaIdActividad])
REFERENCES [dbo].[tblActividades] ([idActividad])
GO
ALTER TABLE [dbo].[tblRegistroAsistencias]  WITH CHECK ADD FOREIGN KEY([tipoDocumentoAprendiz], [numeroDocumentoAprendiz])
REFERENCES [dbo].[tblAprendices] ([tipoDocumentoAprendiz], [numeroDocumentoAprendiz])
GO
ALTER TABLE [dbo].[tblRegistroAsistencias]  WITH CHECK ADD FOREIGN KEY([tipoDocumentoBienestar], [numeroDocumentoBienestar])
REFERENCES [dbo].[tblBienestar] ([tipoDocumentoBienestar], [numeroDocumentoBienestar])
GO
ALTER TABLE [dbo].[tblActividades]  WITH CHECK ADD CHECK  (([categoria]='Psicosocial' OR [categoria]='Salud' OR [categoria]='Deportes' OR [categoria]='Cultura' OR [categoria]='Bienestar'))
GO
ALTER TABLE [dbo].[tblBienestar]  WITH CHECK ADD CHECK  (([cargo]='Trabajador Social' OR [cargo]='Psicólogo' OR [cargo]='Líder de Bienestar' OR [cargo]='Apoyo en Salud' OR [cargo]='Apoyo en Deportes' OR [cargo]='Apoyo en Cultura'))
GO
ALTER TABLE [dbo].[tblFichas]  WITH CHECK ADD CHECK  (([jornada]='Mixta' OR [jornada]='Diurna'))
GO
ALTER TABLE [dbo].[tblUsuarios]  WITH CHECK ADD CHECK  (([tipoDocumento]='PPT' OR [tipoDocumento]='PEP' OR [tipoDocumento]='TI' OR [tipoDocumento]='CE' OR [tipoDocumento]='CC'))
GO
ALTER TABLE [dbo].[tblUsuarios]  WITH CHECK ADD CHECK  (([rol]='Bienestar' OR [rol]='Aprendiz' OR [rol]='Administrador'))
GO
/****** Object:  StoredProcedure [dbo].[spCheckCurrentPassword]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Procedimiento almacenado para verificar si la contraseña actual de un usuario coincide con la registrada en la base de datos

CREATE PROCEDURE [dbo].[spCheckCurrentPassword]
    @TipoDocumento VARCHAR(10),
    @NumeroDocumento VARCHAR(20),
    @ClaveActual VARCHAR(150),
    @PasswordExists BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Verificar si la contraseña actual existe en la tabla
    IF EXISTS (SELECT 1 FROM tblUsuarios WHERE TipoDocumento = @TipoDocumento AND NumeroDocumento = @NumeroDocumento AND clave = @ClaveActual)
    BEGIN
        SET @PasswordExists = 1; -- La contraseña actual existe
    END
    ELSE
    BEGIN
        SET @PasswordExists = 0; -- La contraseña actual no existe
    END
END
GO
/****** Object:  StoredProcedure [dbo].[spConsultActivityByCategory]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--EXEC spConsultActivityByCategory @Categoria = 'Cultura'

--Procedimiento almacenado que permite consultar registros de la tabla tblActividades por Categoria
CREATE PROCEDURE [dbo].[spConsultActivityByCategory]
	@Categoria varchar(20)
AS
SET NOCOUNT OFF;
BEGIN
    SELECT 
	act.idActividad, 
	act.nombreActividad, 
	act.categoria,
	act.horasAsignadas,
	act.descripcion,
	act.fechaInicio,
	act.fechaFinalizacion, 
	CONCAT(us.nombres,' ',us.apellidos) AS 'responsable',
	bie.cargo
FROM tblActividades act
INNER JOIN tblBienestar bie 
	ON bie.tipoDocumentoBienestar = act.tipoDocumentoBienestar AND
	   bie.numeroDocumentoBienestar = act.numeroDocumentoBienestar
INNER JOIN tblUsuarios us 
	ON us.tipoDocumento = bie.tipoDocumentoBienestar AND 
	   us.numeroDocumento = bie.numeroDocumentoBienestar	
    WHERE categoria = @Categoria
    ORDER BY idActividad DESC;
END;
GO
/****** Object:  StoredProcedure [dbo].[spConsultActivityByCategoryAndDate]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--EXEC spConsultActivityByCategoryAndDate @Categoria = 'Psicosocial', @FechaInicio = '2023-10-01', @FechaFinalizacion = '2023-12-15'

--Procedimiento almacenado que permite consultar registros de la tabla tblActividades por Categoria
CREATE PROCEDURE [dbo].[spConsultActivityByCategoryAndDate]
    @Categoria varchar(20),
    @FechaInicio date,
    @FechaFinalizacion date
AS
SET NOCOUNT OFF;
BEGIN
    SELECT 
		act.idActividad, 
		act.nombreActividad, 
		act.categoria,
		act.horasAsignadas,
		act.descripcion,
		act.fechaInicio,
		act.fechaFinalizacion, 
		CONCAT(us.nombres,' ',us.apellidos) AS 'responsable',
		bie.cargo
	FROM tblActividades act
		INNER JOIN tblBienestar bie 
			ON bie.tipoDocumentoBienestar = act.tipoDocumentoBienestar AND
			   bie.numeroDocumentoBienestar = act.numeroDocumentoBienestar
		INNER JOIN tblUsuarios us 
			ON us.tipoDocumento = bie.tipoDocumentoBienestar AND 
			   us.numeroDocumento = bie.numeroDocumentoBienestar	
    WHERE (categoria = @Categoria) AND (
		(fechaInicio BETWEEN @FechaInicio AND @FechaFinalizacion) 
		OR (fechaFinalizacion BETWEEN @FechaInicio AND @FechaFinalizacion) 
		OR ((fechaInicio<=@FechaInicio) AND (fechaFinalizacion>=@FechaFinalizacion))
		)
    ORDER BY idActividad DESC;
END;
GO
/****** Object:  StoredProcedure [dbo].[spConsultActivityByDate]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--EXEC spConsultActivityByDate @FechaInicio = '2023-10-01', @FechaFinalizacion = '2023-12-15'

--Procedimiento almacenado que permite consultar registros de la tabla tblActividades por Fechas
CREATE PROCEDURE [dbo].[spConsultActivityByDate]
	@FechaInicio date,
	@FechaFinalizacion date
AS
SET NOCOUNT OFF;
BEGIN
    SELECT 
		act.idActividad, 
		act.nombreActividad, 
		act.categoria,
		act.horasAsignadas,
		act.descripcion,
		act.fechaInicio,
		act.fechaFinalizacion, 
		CONCAT(us.nombres,' ',us.apellidos) AS 'responsable',
		bie.cargo
	FROM tblActividades act
		INNER JOIN tblBienestar bie 
			ON bie.tipoDocumentoBienestar = act.tipoDocumentoBienestar AND
			   bie.numeroDocumentoBienestar = act.numeroDocumentoBienestar
		INNER JOIN tblUsuarios us 
			ON us.tipoDocumento = bie.tipoDocumentoBienestar AND 
			   us.numeroDocumento = bie.numeroDocumentoBienestar	
	WHERE (fechaInicio BETWEEN @FechaInicio AND @FechaFinalizacion) OR 
		(fechaFinalizacion BETWEEN @FechaInicio AND @FechaFinalizacion) OR 
		((fechaInicio<=@FechaInicio) AND (fechaFinalizacion>=@FechaFinalizacion))
	ORDER BY idActividad DESC;
END;
GO
/****** Object:  StoredProcedure [dbo].[spConsultAllActivities]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--EXEC [spConsultAllActivities];

--Procedimiento almacenado que permite consultar todos los registros de la tabla tblActividades
CREATE PROCEDURE [dbo].[spConsultAllActivities]
AS
SET NOCOUNT OFF;
BEGIN
	SELECT 
		act.idActividad, 
		act.nombreActividad, 
		act.categoria,
		act.horasAsignadas,
		act.descripcion,
		act.fechaInicio,
		act.fechaFinalizacion, 
		CONCAT(us.nombres,' ',us.apellidos) AS 'responsable',
		bie.cargo
	FROM tblActividades act
	INNER JOIN tblBienestar bie 
		ON bie.tipoDocumentoBienestar = act.tipoDocumentoBienestar AND
		   bie.numeroDocumentoBienestar = act.numeroDocumentoBienestar
	INNER JOIN tblUsuarios us 
		ON us.tipoDocumento = bie.tipoDocumentoBienestar AND 
		   us.numeroDocumento = bie.numeroDocumentoBienestar	
	ORDER BY idActividad DESC;
END;
GO
/****** Object:  StoredProcedure [dbo].[spConsultApprentice]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--EXEC spConsultApprentice @TipoDocumentoAprendiz='CE', @NumeroDocumentoAprendiz='20016062';

--Procedimiento almacenado que permite consultar un aprendiz a partir de la llave primaria
CREATE PROCEDURE [dbo].[spConsultApprentice] 
	@TipoDocumentoAprendiz varchar(10), 
	@NumeroDocumentoAprendiz varchar(15)
AS
BEGIN
	SET NOCOUNT OFF;
	SELECT * FROM tblAprendices
	WHERE tipoDocumentoAprendiz=@TipoDocumentoAprendiz And numeroDocumentoAprendiz=@NumeroDocumentoAprendiz
END;
GO
/****** Object:  StoredProcedure [dbo].[spConsultApprenticesByFile]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--EXEC spConsultApprenticesByFile @CopiaFichaNumero = '2617513';

--Procedimiento almacenado que permite consultar aprendices por ficha
CREATE PROCEDURE [dbo].[spConsultApprenticesByFile]
	@CopiaFichaNumero varchar (20)
AS
BEGIN
	SET NOCOUNT OFF;
	SELECT * FROM tblAprendices WHERE copiaFichaNumero=@CopiaFichaNumero
END;
GO
/****** Object:  StoredProcedure [dbo].[spConsultAssistenceByActivity]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--EXEC spConsultAssistenceByActivity @CopiaIdActividad=15;

--Procedimiento almacenado que permite consultar los registros de asistencia por Actividad
CREATE PROCEDURE [dbo].[spConsultAssistenceByActivity]
	@CopiaIdActividad int
AS
BEGIN
	SET NOCOUNT OFF; 
	SELECT * FROM tblRegistroAsistencias WHERE copiaIdActividad=@CopiaIdActividad;
END;
GO
/****** Object:  StoredProcedure [dbo].[spConsultAssitenceByApprentice]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--EXEC spConsultAssitenceByApprentice @TipoDocumentoAprendiz='CC', @NumeroDocumentoAprendiz='101122334'

--Procedimiento almacenado que permite consultar los registros de asistencia por Aprendiz
CREATE PROCEDURE [dbo].[spConsultAssitenceByApprentice]
	@TipoDocumentoAprendiz varchar(10),
	@NumeroDocumentoAprendiz varchar(20)
AS
BEGIN
	SET NOCOUNT OFF; 
	SELECT * FROM tblRegistroAsistencias WHERE tipoDocumentoAprendiz=@TipoDocumentoAprendiz AND numeroDocumentoAprendiz=@NumeroDocumentoAprendiz;
END;
GO
/****** Object:  StoredProcedure [dbo].[spConsultContacts]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--EXEC [spConsultContacts];
-- Procedimiento almacenado que permite consultar todos los registros de la tabla tblBienestar
CREATE PROCEDURE [dbo].[spConsultContacts]
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT CONCAT(us.nombres, ' ', us.apellidos) AS NombreCompleto, bie.cargo, us.correo
	FROM tblUsuarios us
	INNER JOIN tblBienestar bie ON bie.tipoDocumentoBienestar = us.tipoDocumento AND bie.numeroDocumentoBienestar = us.numeroDocumento
	WHERE us.rol = 'Bienestar'
	ORDER BY CASE WHEN bie.cargo = 'Líder de Bienestar' THEN 1 ELSE 2 END, bie.cargo; 

END
GO
/****** Object:  StoredProcedure [dbo].[spConsultFile]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--Consulta una ficha en particular de la lista de fichas existentes
--EXEC spConsultFile @FichaNumero = 'D2617540';


--Procedimiento almacenado que permite consultar una ficha de la tblFichas a partir de su llave primaria
CREATE PROCEDURE [dbo].[spConsultFile] 
	@FichaNumero varchar(20)
AS
BEGIN
	SET NOCOUNT OFF;
	SELECT * FROM tblFichas WHERE fichaNumero=@FichaNumero;
END;
GO
/****** Object:  StoredProcedure [dbo].[spDeleteActivity]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--EXEC spDeleteActivity @IdActividad=26;

--Procedimiento almacenado que permite eliminar un registro de la tabla tblActividades
CREATE PROCEDURE [dbo].[spDeleteActivity]
	@IdActividad int
AS
BEGIN
	DELETE FROM tblActividades
	WHERE idActividad = @IdActividad
END;
GO
/****** Object:  StoredProcedure [dbo].[spDeleteApprentice]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--EXEC spDeleteApprentice @TipoDocumentoAprendiz='CE', @NumeroDocumentoAprendiz='20016062';

--Procedimiento almacenado que permite eliminar un registro de la tabla tblAprendices
CREATE PROCEDURE [dbo].[spDeleteApprentice]
	@TipoDocumentoAprendiz varchar(10),
	@NumeroDocumentoAprendiz varchar(20)
AS
BEGIN
	DELETE FROM tblAprendices
	WHERE tipoDocumentoAprendiz=@TipoDocumentoAprendiz AND numeroDocumentoAprendiz=@NumeroDocumentoAprendiz;
END;
GO
/****** Object:  StoredProcedure [dbo].[spDeleteAssitence]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- EXEC spDeleteAssitence @IdAsistencia = ;

--Procedimiento almacenado que permite eliminar un registro de la tabla tblRegistroAsistencias
CREATE PROCEDURE [dbo].[spDeleteAssitence]
	@IdAsistencia int
AS
BEGIN
	DELETE tblRegistroAsistencias
	WHERE idAsistencia=@IdAsistencia;
END;
GO
/****** Object:  StoredProcedure [dbo].[spDeleteBienestar]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--EXEC spDeleteBienestar @TipoDocumentoBienestar='CE', @NumeroDocumentoBienestar='52513553';

--Procedimiento almacenado que permite eliminar un registro de la tblBienestar
CREATE PROCEDURE [dbo].[spDeleteBienestar]
	@TipoDocumentoBienestar varchar(10),
	@NumeroDocumentoBienestar varchar(20)
AS
BEGIN
	DELETE FROM tblBienestar
	WHERE tipoDocumentoBienestar=@TipoDocumentoBienestar AND numeroDocumentoBienestar=@NumeroDocumentoBienestar;
END;
GO
/****** Object:  StoredProcedure [dbo].[spDeleteFile]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--EXEC spDeleteFile @FichaNumero='D2617540';

--Procedimiento almacenado que permite eliminar un registro de la tabla tblFichas
CREATE PROCEDURE [dbo].[spDeleteFile]
	@FichaNumero varchar(20)
AS
BEGIN
	DELETE FROM tblFichas
	WHERE fichaNumero=@FichaNumero;
END;
GO
/****** Object:  StoredProcedure [dbo].[spDeleteUser]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--EXEC spEliminarUsuario @TipoDocumento='CC', @NumeroDocumento='456789015';

--Procedimiento almacenado que permite eliminar un registro de la tabla tblUsuarios
CREATE PROCEDURE [dbo].[spDeleteUser]
	@TipoDocumento varchar(10),
	@NumeroDocumento varchar(20)
AS
BEGIN
	DELETE FROM tblUsuarios
	WHERE tipoDocumento=@TipoDocumento AND numeroDocumento=@NumeroDocumento;
END;
GO
/****** Object:  StoredProcedure [dbo].[spGetUserByEmailAndToken]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spGetUserByEmailAndToken]

    @Correo VARCHAR(100),
    @Token_recuperacion VARCHAR(250)
AS
BEGIN
    SELECT
        correo,
        token_recuperacion,
        fecha_expiracion_token
    FROM
        tblUsuarios
    WHERE
        correo = @Correo AND
        token_recuperacion = @Token_recuperacion
END
GO
/****** Object:  StoredProcedure [dbo].[spInsertActivity]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--EXEC spInsertActivity @NombreActividad='Spelling Bee', @Categoria='Cultura', @HorasAsignadas=20, @Descripcion='First Spelling Bee to be held in the library at 2pm', @FechaInicio='2024-03-14', @FechaFinalizacion='2024-03-14', @TipoDocumentoBienestar='CE', @NumeroDocumentoBienestar='234567890';

--Procedimiento almacenado que permite insertar un registro en la tabla tblActividades
CREATE PROCEDURE [dbo].[spInsertActivity]
	@NombreActividad varchar(200),
	@Categoria varchar(20),
	@HorasAsignadas int,
	@Descripcion varchar (1000),
	@FechaInicio date,
	@FechaFinalizacion date,
	@TipoDocumentoBienestar varchar(10),
	@NumeroDocumentoBienestar varchar(20)
AS
BEGIN
	SET NOCOUNT OFF; 
	INSERT INTO tblActividades(
		nombreActividad, 
		categoria, 
		horasAsignadas, 
		descripcion, 
		fechaInicio, 
		fechaFinalizacion, 
		tipoDocumentoBienestar,
		numeroDocumentoBienestar)
	VALUES(
		@NombreActividad, 
		@Categoria, 
		@HorasAsignadas,
		@Descripcion, 
		@FechaInicio, 
		@FechaFinalizacion, 
		@TipoDocumentoBienestar, 
		@NumeroDocumentoBienestar)
END;
GO
/****** Object:  StoredProcedure [dbo].[spInsertApprentice]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--Inicialmente se inserta un Usuario de Bienestar en la tabla tblUsuarios
--DECLARE @Registrado bit, @Mensaje varchar(20)
--EXEC spInsertarUsuario @TipoDocumento='CE', @NumeroDocumento='20016062', @Nombres='Inés', @Apellidos='Martinez', @Correo='ines.martinez@soy.sena.edu.co', @Clave='Ines123', @Rol='Aprendiz', @Registrado=@Registrado output, @Mensaje=@Mensaje Output;
--SELECT @Registrado, @Mensaje;

--Luego si se procede a insertar el usuario-Aprendiz en el spInsertApprentice
--EXEC spInsertApprentice @TipoDocumentoAprendiz='CE', @NumeroDocumentoAprendiz='20016062', @CopiaFichaNumero='2617531';

---Procedimiento almacenado que permite insertar un registro en la tblAprendices con totalHorasAsistidas null
CREATE PROCEDURE [dbo].[spInsertApprentice]
	@TipoDocumento varchar(10),
	@NumeroDocumento varchar(20),
	@Nombres varchar(80),
	@Apellidos varchar(80),
	@Correo varchar(100),
	@Clave varchar(150),
	@CopiaFichaNumero varchar (20),
	@Registrado BIT OUTPUT,
	@Mensaje varchar(20) output
AS
BEGIN
	SET NOCOUNT OFF;
	IF(NOT EXISTS(SELECT * FROM tblUsuarios WHERE tipoDocumento=@TipoDocumento AND numeroDocumento=@NumeroDocumento))
	BEGIN
		INSERT INTO tblUsuarios(tipoDocumento, numeroDocumento, nombres, apellidos, correo, clave, rol)
		VALUES(@TipoDocumento, @NumeroDocumento, @Nombres, @Apellidos, @Correo, @Clave, 'Aprendiz')
		INSERT INTO tblAprendices(tipoDocumentoAprendiz, numeroDocumentoAprendiz, copiaFichaNumero)
		VALUES(@TipoDocumento, @NumeroDocumento, @CopiaFichaNumero)
		SET @Registrado=1
		SET @Mensaje='Usuario registrado'
	END;
	ELSE
	BEGIN
		SET @Registrado=0
		SET @Mensaje='Correo ya existe'
	END
END;
GO
/****** Object:  StoredProcedure [dbo].[spInsertAssitence]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--EXEC spInsertAssitence @CopiaIdActividad=12 , @TipoDocumentoAprendiz='CE', @NumeroDocumentoAprendiz='567890120';

--Procedimiento almacenado que permite ingresar un registro a la tabla tblRegistroAsistencias 
--con estadoValidacionBienestar Null, fechaValidacion Null, horasAsistidas , 
--tipoDocumentoBienestar Null y numeroDocumentoBienestar Null
CREATE PROCEDURE [dbo].[spInsertAssitence]
	@CopiaIdActividad int,
	@TipoDocumentoAprendiz varchar(10),
	@NumeroDocumentoAprendiz varchar(20)
AS
BEGIN
	SET NOCOUNT OFF; 
	INSERT INTO tblRegistroAsistencias (copiaIdActividad, tipoDocumentoAprendiz, numeroDocumentoAprendiz)
	VALUES(@CopiaIdActividad, @TipoDocumentoAprendiz, @NumeroDocumentoAprendiz)
END;
GO
/****** Object:  StoredProcedure [dbo].[spInsertBienestar]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--Inicialmente se inserta un Usuario de Bienestar en la tabla tblUsuarios
--DECLARE @Registrado bit, @Mensaje varchar(20)
--EXEC spInsertarUsuario @TipoDocumento='CE', @NumeroDocumento='52513553', @Nombres='Rocio', @Apellidos='Mantilla', @Correo='rocio.mantilla@soy.sena.edu.co', @Clave='Rocio123', @Rol='Bienestar', @Registrado=@Registrado output, @Mensaje=@Mensaje Output;
--SELECT @Registrado, @Mensaje;

--Luego si se procede a insertar el usuario-bienestar en el spInsertBienestar
--EXEC spInsertBienestar @TipoDocumentoBienestar='CE', @NumeroDocumentoBienestar='52513553', @Cargo='Trabajador Social';

--Procedimiento almacenado que permite insertar un registro en la tabla tblBienestar
CREATE PROCEDURE [dbo].[spInsertBienestar]
	@TipoDocumentoBienestar varchar(10),
	@NumeroDocumentoBienestar varchar(20),
	@Cargo varchar(30)
AS
BEGIN
	SET NOCOUNT OFF;
	INSERT INTO tblBienestar(tipoDocumentoBienestar, numeroDocumentoBienestar, cargo)
	VALUES(@TipoDocumentoBienestar, @NumeroDocumentoBienestar, @Cargo)
END;
GO
/****** Object:  StoredProcedure [dbo].[spInsertFile]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--EXEC spInsertFile @FichaNumero='D2617540',  @NombrePrograma= 'Tecnólogo Desarrollo y Adaptación de Prótesis y Ortesis', @FechaInicio='2024-02-15', @FechaFinalizacion='2025-02-15', @Jornada= 'Mixta';

--Procedimiento almacenado que introduce un nuevo registro en la tblFichas
CREATE PROCEDURE [dbo].[spInsertFile]
	@FichaNumero varchar(20),
	@NombrePrograma varchar(100),
	@FechaInicio date,
	@FechaFinalizacion date,
	@Jornada varchar(10)
AS
BEGIN
	SET NOCOUNT OFF;
	INSERT INTO tblFichas(fichaNumero, nombrePrograma, fechaInicio, fechaFinalizacion, jornada)	
	VALUES(@FichaNumero, @NombrePrograma, @FechaInicio, @FechaFinalizacion, @Jornada);
END;
GO
/****** Object:  StoredProcedure [dbo].[spInsertUser]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--DECLARE @Registrado bit, @Mensaje varchar(20)
--EXEC spInsertUser @TipoDocumento='CC', @NumeroDocumento='456789015', @Nombres='Laura', @Apellidos='Martínez', @Correo='laura.martinez@soy.sena.edu.co', @Clave='Laura123', @Rol='Aprendiz', @Registrado=@Registrado output, @Mensaje=@Mensaje Output;
--SELECT @Registrado, @Mensaje;


--Procedimiento almacenado que permite insertar o registar un usuario en la tabla tblUsuarios previa validación en la base de datos
CREATE PROCEDURE [dbo].[spInsertUser]
	@TipoDocumento varchar(10),
	@NumeroDocumento varchar(20),
	@Nombres varchar(80),
	@Apellidos varchar(80),
	@Correo varchar(100),
	@Clave varchar(150),
	@Rol varchar(20),
	@Registrado BIT OUTPUT,
	@Mensaje varchar(20) output
AS
BEGIN
	SET NOCOUNT OFF;
	IF(NOT EXISTS(SELECT * FROM tblUsuarios WHERE tipoDocumento=@TipoDocumento AND numeroDocumento=@NumeroDocumento))
	BEGIN
		INSERT INTO tblUsuarios(tipoDocumento, numeroDocumento, nombres, apellidos, correo, clave, rol)
		VALUES(@TipoDocumento, @NumeroDocumento, @Nombres, @Apellidos, @Correo, @Clave, @Rol)
		SET @Registrado=1
		SET @Mensaje='Usuario registrado'
	END;
	ELSE
	BEGIN
		SET @Registrado=0
		SET @Mensaje='Correo ya existe'
	END
END;
GO
/****** Object:  StoredProcedure [dbo].[spUpdateActivity]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--EXEC spUpdateActivity @IdActividad=26, @NombreActividad='ADSO Spelling Bee', @Categoria='Cultura', @HorasAsignadas=25, @Descripcion='First Spelling Bee to be held in the library at 2pm', @FechaInicio='2024-02-08', @FechaFinalizacion='2024-02-08', @TipoDocumentoBienestar='CE', @NumeroDocumentoBienestar='234567890'; 


--Procedimiento almacenado que permite actualizar un registro de la tabla tblActividades
CREATE PROCEDURE [dbo].[spUpdateActivity]
	@IdActividad int,
	@NombreActividad varchar(200),
	@Categoria varchar(20),
	@HorasAsignadas int,
	@Descripcion varchar (1000),
	@FechaInicio date,
	@FechaFinalizacion date,
	@TipoDocumentoBienestar varchar(10),
	@NumeroDocumentoBienestar varchar(20)
AS
BEGIN
	UPDATE tblActividades
	SET
		nombreActividad=@NombreActividad,
		categoria=@Categoria,
		horasAsignadas=@HorasAsignadas,
		descripcion=@Descripcion,
		fechaInicio=@FechaInicio,
		fechaFinalizacion=@FechaFinalizacion,
		tipoDocumentoBienestar=@TipoDocumentoBienestar,
		numeroDocumentoBienestar=@NumeroDocumentoBienestar
	WHERE idActividad=@IdActividad;
END;
GO
/****** Object:  StoredProcedure [dbo].[spUpdateApprentice]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--EXEC spUpdateApprentice @TipoDocumentoAprendiz='CE', @NumeroDocumentoAprendiz='20016062', @CopiaFichaNumero = '2617520';

--Procedimiento almacenado que permite actualizar la ficha de un registro de la tabla tblAprendices
CREATE PROCEDURE [dbo].[spUpdateApprentice]
	@TipoDocumentoAprendiz varchar(10),
	@NumeroDocumentoAprendiz varchar(20),
	@CopiaFichaNumero varchar(20)
AS
BEGIN
	UPDATE tblAprendices
	SET
	copiaFichaNumero=@CopiaFichaNumero
	WHERE tipoDocumentoAprendiz=@TipoDocumentoAprendiz AND numeroDocumentoAprendiz=@NumeroDocumentoAprendiz;
END;
GO
/****** Object:  StoredProcedure [dbo].[spUpdateApprenticeHours]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--EXEC spUpdateApprenticeHours @TipoDocumentoAprendiz='CE', @NumeroDocumentoAprendiz='20016062', @TotalHorasAsistidas=40;

--Procedimiento almacenado que permite actualizar la cantidad de horas de bienestar de un registro de la tabla tblAprendices
CREATE PROCEDURE [dbo].[spUpdateApprenticeHours]
	@TipoDocumentoAprendiz varchar(10),
	@NumeroDocumentoAprendiz varchar(20),
	@TotalHorasAsistidas int
AS
BEGIN
	UPDATE tblAprendices
	SET
	totalHorasAsistidas=@TotalHorasAsistidas
	WHERE tipoDocumentoAprendiz=@TipoDocumentoAprendiz AND numeroDocumentoAprendiz=@NumeroDocumentoAprendiz;
END;
GO
/****** Object:  StoredProcedure [dbo].[spUpdateAutomaticAsistence]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--EXEC spUpdateAutomaticAsistence @IdAsistencia 1, @EstadoValidacionBienestar =1, @TipoDocumentoBienestar 'CC', 	@NumeroDocumentoBienestar='778901237';
--procedimiento almacenado que permite modificar el estadoValidacionBienestar de un registro
--en la tabla tblRegistroAsistencias agregando la cantidad de horas asistidas de
--forma automatica desde el registro de la actividad en la tabla tblActividades en el campo 
--horasAsignadas

CREATE PROCEDURE [dbo].[spUpdateAutomaticAsistence]
	@IdAsistencia int,
	@EstadoValidacionBienestar bit,
	@TipoDocumentoBienestar varchar(10),
	@NumeroDocumentoBienestar varchar(20)
AS
BEGIN
	declare @CopiaIdActividad int;
	select @CopiaIdActividad=copiaIdActividad from tblRegistroAsistencias where idAsistencia=@IdAsistencia;
	declare @HorasAsistidas	int;
	select @HorasAsistidas=horasAsignadas from tblActividades Where idActividad=@CopiaIdActividad;
	UPDATE tblRegistroAsistencias
	SET estadoValidacionBienestar=@EstadoValidacionBienestar,
		fechaValidacion=getdate(),
		horasAsistidas=@HorasAsistidas,
		tipoDocumentoBienestar=@TipoDocumentoBienestar,
		numeroDocumentoBienestar=@NumeroDocumentoBienestar
	WHERE idAsistencia=@IdAsistencia;
END;
GO
/****** Object:  StoredProcedure [dbo].[spUpdateBienestar]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--EXEC spUpdateBienestar @TipoDocumentoBienestar='CE', @NumeroDocumentoBienestar='52513553', @Cargo='Apoyo en Deportes';

--Procedimiento almacenado que permite actualizar el cargo de un registro de la tblBienestar
CREATE PROCEDURE [dbo].[spUpdateBienestar]
	@TipoDocumentoBienestar varchar(10),
	@NumeroDocumentoBienestar varchar(20),
	@Cargo varchar(30)
AS
BEGIN
	UPDATE tblBienestar
	SET cargo=@Cargo
	WHERE tipoDocumentoBienestar=@TipoDocumentoBienestar AND numeroDocumentoBienestar=@NumeroDocumentoBienestar;
END;
GO
/****** Object:  StoredProcedure [dbo].[spUpdateForgottenPassword]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--EXEC spUpdateForgottenPassword @Correo ='rosa.zamora@soy.sena.edu.co', @NuevaClave='3252d7b2a3d2e2b9cf9a97806a54a38284e248e7d768ceef307853a1f3de7257'; 

--Procedimiento almacenado que permite actualizar la clave de un registro de la tabla tblUsuarios
CREATE PROCEDURE [dbo].[spUpdateForgottenPassword]
	   @Correo VARCHAR(100),
	   @NuevaClave VARCHAR(250)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE tblUsuarios
    SET clave = @NuevaClave,
        token_recuperacion = NULL,
        fecha_expiracion_token = NULL
    WHERE correo = @Correo;
END
GO
/****** Object:  StoredProcedure [dbo].[spUpdateManualAssistance]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--EXEC spUpdateManualAssistance @IdAsistencia=1, @EstadoValidacionBienestar=1, @HorasAsistidas=20, @TipoDocumentoBienestar='CC', @NumeroDocumentoBienestar='778901237';

--procedimiento almacenado que permite modificar el estadoValidacionBienestar de un registro 
--en la tabla tblRegistroAsistencias agregando la cantidad de horas asistidas de forma manual

CREATE PROCEDURE [dbo].[spUpdateManualAssistance]
	@IdAsistencia int,
	@EstadoValidacionBienestar bit,
	@HorasAsistidas int,
	@TipoDocumentoBienestar varchar(10),
	@NumeroDocumentoBienestar varchar(20)
AS
BEGIN
	UPDATE tblRegistroAsistencias
	SET estadoValidacionBienestar=@EstadoValidacionBienestar,
		fechaValidacion=getdate(),
		horasAsistidas=@HorasAsistidas,
		tipoDocumentoBienestar=@TipoDocumentoBienestar,
		numeroDocumentoBienestar=@NumeroDocumentoBienestar
	WHERE idAsistencia=@IdAsistencia;
END;
GO
/****** Object:  StoredProcedure [dbo].[spUpdatePassword]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--EXEC spUpdatePassword @TipoDocumento='TI', @NumeroDocumento='955566778',  @ClaveActual='12345', @ClaveNueva='Fabian123'; 

--Procedimiento almacenado que permite actualizar la clave de un registro de la tabla tblUsuarios
CREATE PROCEDURE [dbo].[spUpdatePassword]
	@TipoDocumento varchar(10),
	@NumeroDocumento varchar(20),
	@ClaveActual varchar(150),
	@ClaveNueva varchar (150)
AS
BEGIN
	UPDATE tblUsuarios
	SET
		clave=@ClaveNueva
	WHERE tipoDocumento=@TipoDocumento AND numeroDocumento=@NumeroDocumento AND clave=@ClaveActual;
END;
GO
/****** Object:  StoredProcedure [dbo].[spUpdateRecoveryToken]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--DECLARE @Correo VARCHAR(100) = 'olga.vargas@soy.sena.edu.co';
--DECLARE @Token_recuperacion VARCHAR(250) = 'e28763d1f0c7426c8d2c6a75c2e7b863';
--DECLARE @Fecha_expiracion_token DATETIME = '2024-03-21 18:30:00';

--EXEC [dbo].[spUpdateRecoveryToken] 
    --@Correo = @Correo,
    --@Token_recuperacion = @Token_recuperacion,
    --@Fecha_expiracion_token = @Fecha_expiracion_token;

--SELECT @Token_recuperacion AS TokenRecuperacion, @Fecha_expiracion_token AS FechaExpiracionToken;


CREATE PROCEDURE [dbo].[spUpdateRecoveryToken]
    @Correo VARCHAR(100),
    @Token_recuperacion VARCHAR(250),
    @Fecha_expiracion_token DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    -- Actualizar la tabla con el nuevo token y fecha de expiración
    UPDATE tblUsuarios
    SET token_recuperacion = @Token_recuperacion,
        fecha_expiracion_token = @Fecha_expiracion_token
    WHERE correo = @Correo;
END;
GO
/****** Object:  StoredProcedure [dbo].[spValidateEmail]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--DECLARE @EmailExists BIT; 
--EXEC spValidateEmail @Correo = 'lui.sanchez@soy.sena.edu.co', @EmailExists = @EmailExists OUTPUT;
--SELECT @EmailExists AS EmailExistsResult;

-- Procedimiento para verificar si un correo electrónico existe

CREATE PROCEDURE [dbo].[spValidateEmail]
    @Correo VARCHAR(100),
    @EmailExists BIT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	-- Verificar si el correo existe en la tabla
    IF EXISTS (SELECT 1 FROM tblUsuarios WHERE correo = @Correo)
    BEGIN
        SET @EmailExists = 1; -- El correo electrónico existe
    END
    ELSE
    BEGIN
        SET @EmailExists = 0; -- El correo electrónico no existe
    END
END
GO
/****** Object:  StoredProcedure [dbo].[spValidateRecoveryToken]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spValidateRecoveryToken]
    @Correo VARCHAR(100),
    @Token_recuperacion VARCHAR(250),
    @TokenValido BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Fecha_expiracion_token DATETIME;

    SELECT @Fecha_expiracion_token = fecha_expiracion_token
    FROM tblUsuarios
    WHERE correo = @Correo AND token_recuperacion = @Token_recuperacion;

    IF @Fecha_expiracion_token IS NULL
    BEGIN
        SET @TokenValido = 0; -- Token no encontrado o inválido
        RETURN;
    END

    IF @Fecha_expiracion_token < GETDATE()
    BEGIN
        SET @TokenValido = 0; -- Token expirado
        RETURN;
    END

    SET @TokenValido = 1; -- Token válido
END
GO
/****** Object:  StoredProcedure [dbo].[spValidateUser]    Script Date: 24/08/2024 7:11:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--EXEC spValidateUser @TipoDocumento = 'CC', @NumeroDocumento = '456789015', @Clave = 'dbbf038fada6d9cf8b3b3b730d3bb95984ce6a517e72a578818f51c20e799d5f';
--EXEC spValidateUser @TipoDocumento = 'CC', @NumeroDocumento = '126456789', @Clave = '377d24b40547a9cf506fe5087e2a41e4c2f9770a97dcb9a6925465e724f4f9f8';
--EXEC spValidateUser @TipoDocumento = 'CC', @NumeroDocumento = '101122334', @Clave = '4127e092b7986e47ad06a7e14519329e66c4f423173712a9ce090d95fe7a9b79';

CREATE PROCEDURE [dbo].[spValidateUser]
	@TipoDocumento varchar(10),
	@NumeroDocumento varchar(20),
	@Clave varchar(250)
AS
BEGIN
	IF(EXISTS(SELECT * FROM tblUsuarios WHERE tipoDocumento=@TipoDocumento AND numeroDocumento=@NumeroDocumento AND clave=@Clave))
		--SELECT 1
		SELECT tipoDocumento, numeroDocumento, nombres, apellidos, correo, clave, rol FROM tblUsuarios WHERE tipoDocumento=@TipoDocumento  AND numeroDocumento=@NumeroDocumento AND clave=@Clave
	ELSE
		SELECT null
END;
GO
USE [master]
GO
ALTER DATABASE [plataformaMotVer6] SET  READ_WRITE 
GO
