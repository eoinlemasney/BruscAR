# BruscAR

An augmented reality (AR) mobile application built with Unity that helps users identify and learn about recycling different items through image recognition and interactive 3D objects.

## Overview

BruscAR is an educational AR app that uses image tracking to identify recyclable, compostable, and general waste items. When users scan specific images with their mobile device camera, the app displays 3D AR objects (recycling bins) and provides information about proper disposal methods.

## Features

### Core Functionality
- **AR Image Tracking**: Recognises specific reference images and displays corresponding 3D recycling bins
- **Interactive 3D Objects**: Tap on AR objects to get detailed recycling information
- **Item Classification**: Automatically categorises items as Recyclable, Compostable, or General Waste
- **Audio Feedback**: Different sounds for different waste types (recycling sound, beep for general waste, bing for compost)
- **Particle Effects**: Visual feedback when interacting with AR objects

### User Interface
- **Main Menu**: Simple navigation to different app sections
- **Scan Mode**: AR camera view for image recognition
- **Search Functionality**: Text-based search through recyclable items database
- **User Statistics**: Track total items scanned and categorised counts
- **Tips & Tricks**: Educational content and external links
- **Account Management**: View scanning history and statistics

### Supported Items
The app currently recognises:
- **Recyclable Items**: Coke cans, magazines, etc.
- **Compostable Items**: Barry's Tea bags, food waste
- **General Waste**: Terry's Chocolate Orange packaging, non-recyclable items

## Technical Details

### Unity Version
- **Unity 2019.4.12f1** (LTS)

### Key Dependencies
- **AR Foundation**: For AR image tracking functionality
- **XR Plugin Management**: AR/VR support
- **JsonDotNet**: JSON data parsing for item database
- **TextMesh Pro**: Enhanced text rendering

### Architecture

#### Core Scripts
- `ImageTracking.cs`: Main AR functionality, image recognition, and 3D object management
- `MainMenu.cs`: Scene navigation and game flow
- `RecycleInfo.cs`: UI management for recycling information panels
- `SearchInput.cs`: Text-based search through recyclable items database
- `UserDetails.cs`: User statistics and session data management
- `TipsTricks.cs`: External link management for educational content

#### Data Management
- **JSON Database**: `recycle_database.json` contains item information (name, image reference, recyclability, instructions)
- **PlayerPrefs**: Persistent storage for user statistics across sessions
- **User Data**: Local file storage for additional user information

#### Scene Structure
- `MainMenu.unity`: App entry point
- `Scan.unity`: AR scanning interface
- `SearchForItem.unity`: Text search functionality
- `MyAccount.unity`: User statistics and profile
- `TipsAndTricks.unity`: Educational content
- `WhereToRecycle.unity`: Location-based recycling information

## Setup Instructions

### Prerequisites
- Unity 2019.4.12f1 or compatible LTS version
- Android/iOS development environment
- AR Foundation package installed
- XR Plugin Management configured

### Installation
1. Clone the repository
2. Open the project in Unity 2019.4.12f1
3. Ensure AR Foundation and XR Plugin Management packages are installed
4. Configure build settings for your target platform (Android/iOS)
5. Build and deploy to your mobile device

### AR Setup
1. Add reference images to the AR Reference Image Library
2. Configure image tracking settings in AR Tracked Image Manager
3. Assign corresponding 3D prefabs to the `placeablePrefabs` array in `ImageTracking.cs`

## Usage

### Scanning Items
1. Launch the app and navigate to "Scan" mode
2. Point your device camera at a supported reference image
3. Wait for the AR object to appear
4. Tap the 3D object for detailed recycling information

### Searching Items
1. Navigate to "Search" mode
2. Type the name of an item in the search bar
3. View recycling information and instructions

### Viewing Statistics
1. Go to "My Account" to view scanning history
2. See total items scanned and breakdown by category

## Customisation

### Adding New Items
1. Add reference images to the AR Reference Image Library
2. Update the `ReturnItemName()` method in `ImageTracking.cs` for new item names
3. Add corresponding entries to `recycle_database.json`
4. Create 3D prefabs for new waste categories if needed

### Modifying UI
- UI elements are managed through Unity's Canvas system
- Text components are linked to scripts for dynamic content updates
- Images and materials can be customised in the Materials folder

## File Structure

```
Assets/
├── Images/                 # Reference images for AR tracking
├── ItemsData/             # JSON database of recyclable items
├── Prefabs/               # 3D AR objects (recycling bins)
├── Scripts/               # Core application logic
├── Scenes/                # Unity scene files
├── Materials/             # Visual materials and textures
└── Resources/             # Additional assets loaded at runtime
```

## Notes

- The app requires specific reference images to function properly
- AR functionality is limited to devices with camera and AR support
- User data is stored locally on the device
- The app is designed for educational purposes around recycling awareness


