using System;
using Logus.Dominio.ObjetosDeValor;

namespace Logus.Dominio.Solicitacoes;

public sealed class ModuloCursado
{
    public Guid Id { get; }

    public string Nome { get; private set; }

    public Nota? Media { get; private set; }

    public ModuloCursado(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            throw new ArgumentException(
                "O nome do módulo é obrigatório.",
                nameof(nome));
        }

        Id = Guid.NewGuid();
        Nome = nome.Trim();
    }

    public void Renomear(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            throw new ArgumentException(
                "O nome do módulo é obrigatório.",
                nameof(nome));
        }

        Nome = nome.Trim();
    }

    public void InformarMedia(Nota media)
    {
        if (media is null)
        {
            throw new ArgumentNullException(nameof(media));
        }

        Media = media;
    }

    public void RemoverMedia()
    {
        Media = null;
    }
}