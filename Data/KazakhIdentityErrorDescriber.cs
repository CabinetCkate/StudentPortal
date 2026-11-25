using Microsoft.AspNetCore.Identity;

namespace StudentPortal.Data;

public sealed class KazakhIdentityErrorDescriber : IdentityErrorDescriber
{
    public override IdentityError DuplicateEmail(string email) =>
        new() { Code = nameof(DuplicateEmail), Description = "Бұл email бұрыннан тіркелген." };

    public override IdentityError DuplicateUserName(string userName) =>
        new() { Code = nameof(DuplicateUserName), Description = "Бұл email бұрыннан тіркелген." };

    public override IdentityError PasswordTooShort(int length) =>
        new() { Code = nameof(PasswordTooShort), Description = $"Құпиясөз кемінде {length} таңбадан тұруы керек." };

    public override IdentityError PasswordRequiresDigit() =>
        new() { Code = nameof(PasswordRequiresDigit), Description = "Құпиясөзде кемінде бір сан болуы керек." };

    public override IdentityError PasswordRequiresUpper() =>
        new() { Code = nameof(PasswordRequiresUpper), Description = "Құпиясөзде кемінде бір бас әріп болуы керек." };

    public override IdentityError PasswordRequiresNonAlphanumeric() =>
        new() { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "Құпиясөзде кемінде бір арнайы таңба болуы керек." };

    public override IdentityError PasswordRequiresLower() =>
        new() { Code = nameof(PasswordRequiresLower), Description = "Құпиясөзде кемінде бір кіші әріп болуы керек." };
}
