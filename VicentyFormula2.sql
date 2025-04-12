USE [AdsGeoDB]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[VincentyFormulaSQL2]
(
    @lat1 FLOAT,
    @lon1 FLOAT,
    @lat2 FLOAT,
    @lon2 FLOAT
)
RETURNS FLOAT
AS
BEGIN
    -- Verificación de si los puntos son prácticamente iguales
    IF ABS(@lat1 - @lat2) < 1e-8 AND ABS(@lon1 - @lon2) < 1e-8
    BEGIN
        RETURN 0 -- Retorna 0 si los puntos son efectivamente iguales
    END

    -- Declaración de constantes
    DECLARE @a FLOAT = 6378137,  -- Radio ecuatorial en metros
            @b FLOAT = 6356752.314245, -- Radio polar en metros
            @f FLOAT = 1 / 298.257223563, -- Aplanamiento de la Tierra
            @L FLOAT, @U1 FLOAT, @U2 FLOAT, @lambda FLOAT, @prevLambda FLOAT,
            @iterLimit INT = 100, @sinLambda FLOAT, @cosLambda FLOAT,
            @sinSigma FLOAT, @cosSigma FLOAT, @sigma FLOAT, @sinAlpha FLOAT,
            @cosSqAlpha FLOAT, @cos2SigmaM FLOAT, @C FLOAT, @uSq FLOAT,
            @A2 FLOAT, @B2 FLOAT, @deltaSigma FLOAT, @distance FLOAT;

    -- Conversión de grados a radianes
    SET @lat1 = @lat1 * PI() / 180;
    SET @lat2 = @lat2 * PI() / 180;
    SET @lon1 = @lon1 * PI() / 180;
    SET @lon2 = @lon2 * PI() / 180;

    SET @L = @lon2 - @lon1;

    -- Cálculo de valores U
    SET @U1 = ATN2((1 - @f) * SIN(@lat1), COS(@lat1));
    SET @U2 = ATN2((1 - @f) * SIN(@lat2), COS(@lat2));

    -- Inicialización de lambda
    SET @lambda = @L;
    SET @prevLambda = 0;

    -- Bucle iterativo para la fórmula de Vincenty
    WHILE @iterLimit > 0 AND ABS(@lambda - @prevLambda) > 1e-12
    BEGIN
        SET @sinLambda = SIN(@lambda);
        SET @cosLambda = COS(@lambda);

        SET @sinSigma = SQRT((COS(@U2) * @sinLambda) * (COS(@U2) * @sinLambda) +
                             (COS(@U1) * SIN(@U2) - SIN(@U1) * COS(@U2) * @cosLambda) *
                             (COS(@U1) * SIN(@U2) - SIN(@U1) * COS(@U2) * @cosLambda));

        IF @sinSigma = 0 RETURN 0; -- Puntos coincidentes

        SET @cosSigma = SIN(@U1) * SIN(@U2) + COS(@U1) * COS(@U2) * @cosLambda;
        SET @sigma = ATN2(@sinSigma, @cosSigma);
        SET @sinAlpha = COS(@U1) * COS(@U2) * @sinLambda / @sinSigma;
        SET @cosSqAlpha = 1 - @sinAlpha * @sinAlpha;

        SET @cos2SigmaM = 0;
        IF @cosSqAlpha != 0
            SET @cos2SigmaM = @cosSigma - 2 * SIN(@U1) * SIN(@U2) / @cosSqAlpha;

        SET @C = @f / 16 * @cosSqAlpha * (4 + @f * (4 - 3 * @cosSqAlpha));

        SET @prevLambda = @lambda;
        SET @lambda = @L + (1 - @C) * @f * @sinAlpha *
                      (@sigma + @C * @sinSigma * (@cos2SigmaM + @C * @cosSigma * (-1 + 2 * @cos2SigmaM * @cos2SigmaM)));

        SET @iterLimit = @iterLimit - 1;
    END

    IF @iterLimit = 0 RETURN NULL; -- La fórmula falló en converger

    -- Cálculo final de la distancia
    SET @uSq = @cosSqAlpha * (@a * @a - @b * @b) / (@b * @b);
    SET @A2 = 1 + @uSq / 16384 * (4096 + @uSq * (-768 + @uSq * (320 - 175 * @uSq)));
    SET @B2 = @uSq / 1024 * (256 + @uSq * (-128 + @uSq * (74 - 47 * @uSq)));
    SET @deltaSigma = @B2 * @sinSigma * (@cos2SigmaM + @B2 / 4 * (@cosSigma * (-1 + 2 * @cos2SigmaM * @cos2SigmaM)
                      - @B2 / 6 * @cos2SigmaM * (-3 + 4 * @sinSigma * @sinSigma) * (-3 + 4 * @cos2SigmaM * @cos2SigmaM)));

    SET @distance = @b * @A2 * (@sigma - @deltaSigma);

    RETURN @distance;
END
GO
