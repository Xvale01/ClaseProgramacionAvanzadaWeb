USE [master]
GO

CREATE DATABASE [sabado_db]
GO

USE [sabado_db]
GO

/* ---------------------------------------- TABLAS ---------------------------------------- */

CREATE TABLE [dbo].[tRol](
	[IdRol] [smallint] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
 CONSTRAINT [PK_tRol] PRIMARY KEY CLUSTERED 
(
	[IdRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tUsuario](
	[IdUsuario] [bigint] IDENTITY(1,1) NOT NULL,
	[Correo] [varchar](200) NOT NULL,
	[Contrasenna] [varchar](200) NOT NULL,
	[Nombre] [varchar](200) NOT NULL,
	[IdRol] [smallint] NOT NULL,
	[Estado] [bit] NOT NULL,
 CONSTRAINT [PK_tUsuario] PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tUsuario]  WITH CHECK ADD  CONSTRAINT [FK_tUsuario_tRol] FOREIGN KEY([IdRol])
REFERENCES [dbo].[tRol] ([IdRol])
GO
ALTER TABLE [dbo].[tUsuario] CHECK CONSTRAINT [FK_tUsuario_tRol]
GO


/* --------------------------------- INSERCIONES A TABLAS --------------------------------- */

SET IDENTITY_INSERT [dbo].[tRol] ON 
GO
INSERT [dbo].[tRol] ([IdRol], [Nombre]) VALUES (1, N'Usuario')
GO
INSERT [dbo].[tRol] ([IdRol], [Nombre]) VALUES (2, N'Administrador')
GO
SET IDENTITY_INSERT [dbo].[tRol] OFF
GO

SET IDENTITY_INSERT [dbo].[tUsuario] ON 
GO
INSERT [dbo].[tUsuario] ([IdUsuario], [Correo], [Contrasenna], [Nombre], [IdRol], [Estado]) VALUES (1, N'ecalvo90415@ufide.ac.cr', N'90415', N'Eduardo Calvo Castillo', 1, 1)
GO
SET IDENTITY_INSERT [dbo].[tUsuario] OFF
GO



/* ------------------------------ PROCEDIMIENTOS ALMACENADOS ------------------------------ */

CREATE PROCEDURE [dbo].[RegistrarUsuario]
	@Correo			VARCHAR(200),
    @Contrasenna	VARCHAR(200),
    @NombreUsuario			VARCHAR(200)
AS
BEGIN

	IF NOT EXISTS(SELECT 1 FROM tUsuario WHERE Correo = @Correo)
	BEGIN

		DECLARE @Estado BIT = 1,
				@IdRol  SMALLINT = 1,
				@EsTemporal BIT = 0

		INSERT INTO dbo.tUsuario(Correo,Contrasenna,Nombre,IdRol,Estado,EsTemporal)
		VALUES (@Correo,@Contrasenna,@NombreUsuario,@IdRol,@Estado,@EsTemporal)

	END

END




CREATE PROCEDURE [dbo].[IniciarSesion]
	@Correo			VARCHAR(200),
    @Contrasenna	VARCHAR(200)
AS
BEGIN

	SELECT	IdUsuario,Correo,U.Nombre 'NombreUsuario',U.IdRol,R.Nombre 'NombreRol',Estado, EsTemporal
	  FROM	tUsuario U
	  INNER JOIN tRol R ON U.IdRol = R.IdRol
	  WHERE	Correo = @Correo
		AND Contrasenna = @Contrasenna
		AND Estado = 1

END




CREATE PROCEDURE RecuperarAcceso

	@Correo VARCHAR(200),
	@Contrasenna VARCHAR(200),
	@EsTemporal BIT

AS
BEGIN 

	DECLARE @Consecutivo BIGINT 

	SELECT @Consecutivo = IdUsuario
	FROM tUsuario
	WHERE Correo = @Correo
	AND Estado = 1
	
	IF @Consecutivo IS NOT NULL
	BEGIN
		UPDATE tUsuario
		SET Contrasenna = @Contrasenna,
			EsTemporal = @EsTemporal
		WHERE Correo = @Correo	
	END

	SELECT IdUsuario, Correo, U.Nombre 'NombreUsuario', U.IdRol, R.Nombre 'NombreRol',Estado, EsTemporal 
	FROM tUsuario U
	INNER JOIN tRol R ON U.IdRol = R.IdRol
	WHERE Correo = @Correo
	AND Estado = 1

END 
GO


/* --------------------------------------- CONSULTAS -------------------------------------- */
SELECT * FROM [dbo].[tUsuario]



