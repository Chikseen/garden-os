import SwiftUI

struct PasswordInput: View {
    var placeHolder: String = ""
    @Binding var password: String
    
    var body: some View {
        VStack(){
            Text("Password")
                .frame(maxWidth: /*@START_MENU_TOKEN@*/.infinity/*@END_MENU_TOKEN@*/ , alignment: .leading)
            SecureField(placeHolder, text: $password)
                .textFieldStyle(.roundedBorder)
        }
        .padding(10)
    }
}

#Preview {
    PasswordInput(placeHolder: "Password", password: .constant(""))
}
