import SwiftUI
import Combine

struct EmailInput: View {
    var placeHolder: String = ""
    @Binding var email: String
    
    var body: some View {
        VStack(){
            Text("Email")
                .frame(maxWidth: /*@START_MENU_TOKEN@*/.infinity/*@END_MENU_TOKEN@*/ , alignment: .leading)
            TextField(placeHolder, text: $email)
                .keyboardType(.emailAddress)
                .onReceive(Just(email)) { newValue in
                    let validString = newValue.filter { "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ._-+$!~&=#[]@".contains($0) }
                    if validString != newValue {
                        self.email = validString
                    }
                }
                .textFieldStyle(.roundedBorder)
        }
        .padding(10)
    }
}

#Preview {
    EmailInput(placeHolder: "Email", email: .constant(""))
}
