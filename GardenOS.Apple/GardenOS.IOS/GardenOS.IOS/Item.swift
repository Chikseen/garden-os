//
//  Item.swift
//  GardenOS.IOS
//
//  Created by Tim Keutel on 02.04.24.
//

import Foundation
import SwiftData

@Model
final class Item {
    var timestamp: Date
    
    init(timestamp: Date) {
        self.timestamp = timestamp
    }
}
