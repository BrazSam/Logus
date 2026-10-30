using System;
using System.Collections.Generic;

namespace Logus.Dominio.Solicitacoes;

public static class RegraMediaFinal
{
    public static decimal Calcular(
        IReadOnlyCollection<ModuloCursado> modulos)
    {
        if (modulos is null)
        {
            throw new ArgumentNullException(nameof(modulos));
        }

        if (modulos.Count == 0)
        {
            throw new ArgumentException(
                "Informe pelo menos um módulo.",
                nameof(modulos));
        }

        decimal soma = 0m;

        foreach (var modulo in modulos)
        {
            if (modulo is null)
            {
                throw new ArgumentException(
                    "A coleção contém um módulo inválido.",
                    nameof(modulos));
            }

            if (modulo.Media is null)
            {
                throw new InvalidOperationException(
                    $"O módulo '{modulo.Nome}' está sem média.");
            }

            soma += modulo.Media.Valor;
        }

        decimal mediaCalculada = soma / modulos.Count;

        decimal parteInteira = decimal.Floor(mediaCalculada);
        decimal parteDecimal = mediaCalculada - parteInteira;

        if (parteDecimal >= 0.90m)
        {
            return parteInteira + 1m;
        }

        return mediaCalculada;
    }
}