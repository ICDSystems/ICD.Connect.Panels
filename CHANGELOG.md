# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [10.0.1] - 2023-03-22
###Changed
 - Removed Obfuscation

## [10.0.0] - 2022-07-01
### Changed
 - Moved eHardButton enum to protocol project
 - Updated Crestron SDK to 2.18.96

## [9.3.1] - 2022-06-16
### Added
 - Added hard button control to TSWx70 and TSx70 panels

## [9.3.0] - 2022-05-23
### Added
 - Add device classes for new Crestron 70 series touch panels
	- Tsw570
	- Tsw570P
	- Ts770
	- Ts770Gv
	- Tsw770
	- Tsw770Gv
	- Ts1070
	- Ts1070Gv
	- Tsw1070
	- Tsw1070Gv
 - Added HR-100, HR-150, and HR-310 remote classes

## [9.2.1] - 2021-08-03
### Changed
 - CrestronProjectInfo - Gather all info in a thread, since CrestronEthernetDeviceUtils doesn't start threads any more

## [9.2.0] - 2021-05-14
### Added
 - Device telemetry gathering and touchpanel telemetry gathering and reporting to AbstractTswFt5ButtonAdapter
 - Device telemetry gathering and touchpanel telemetry gathering and reporting to AbstractDgeX00Adapter
 - Device telemetry gathering and touchpanel telemetry gathering and reporting to AbstractTs1542Adapter

### Changed
 - Crestron panels will no longer poll for telemetry if there is no configured network username.
 - Refactored CrestronPro TriListAdapter namespace
 - Fixed a bug where panel servers would resume listening after being disposed 

## [9.1.0] - 2021-01-14
### Changed
 - XPanels ignore Crestron IsOnline feedback since they are used for diagnostics and we don't want to pollute telemetry
 - AbstractTswFt5ButtonAdapter - Removing ip and mac address propreties and using Monitored Device Telemetry instead
 - Fixed null reference exception with DgeX00 Stream Switcher
 - IDgeX00StreamSwitcherControl implements IRouteSwitcherControl

## [9.0.0] - 2020-09-24
### Added
 - Added backlight device controls

### Changed
 - Replaced panel power controls with backlight controls

## [8.0.1] - 2020-08-13
### Changed
 - Telemetry namespace change
 - Updated NetworkPro dependency dlls

## [8.0.0] - 2020-06-19
### Added
 - Added WebPanelServerDevice for hosting WebSocket panels

### Changed
 - MockPanelDevice now implements IMockDevice
 - Using new logging context
 - Panels are now Devices, no longer treating Panels as a distinct subset of devices

## [7.5.1] - 2020-08-06
### Changed
 - Moved DgeX00 interfaces into non-pro project to fix downstream dependency issues

## [7.5.0] - 2020-06-30
### Added
 - Added AbstractDge100Adapter, Dge100 & DmDge200C adapters
 - Added routing controls for DGE devices

### Changed
 - Change Panel Control ID to 10, so Routing control ID can be 0 for DAV support

## [7.4.0] - 2020-05-23
### Changed
 - Updated ConfernceControls to support ConferenceHistory Changes - Start/End renamed, Incoming call direction removed

## [7.3.0] - 2020-03-20
### Added
 - Conference controls implement SupportedConferenceFeatures property

### Changed
 - Using UTC for times

## [7.2.1] - 2019-11-19
### Changed
 - Fixed incompatibility with new conferencing interfaces

## [7.2.0] - 2019-09-16
### Changed
 - Using new GenericBaseUtils to standardize crestron device setup and teardown
 - Updated IPowerDeviceControls to use PowerState

## [7.1.0] - 2019-08-05
### Added
 - Added Console Commands for smart object panels

### Changed
 - Using new JSON serialization/deserialization utils
 - Fixed a bug where JSON was serializing a message with a value instead of a key.

## [7.0.0] - 2019-01-14
### Changed
 - Dialing features refactored to fit new conferencing interfaces

## [6.6.0] - 2020-04-30
### Added
 - Added IP and Hostname properties/telemetry to AbstractTswFt5Button Panels using EthernetReservedSigs extender

## [6.5.0] - 2019-08-05
### Added
 - Added a RouteDestinationControl to the Ts1542CAdapter

## [6.4.0] - 2019-07-16
### Changed
 - Fixed a bug where the panel server was not properly communicating SmartObject ids
 - Changed PowerOn/PowerOff methods to support pre-on/off callbacks

## [6.3.0] - 2019-06-06
### Changed
 - Panel threading improvements, potential deadlock fixes
 - SmartObject optimizations

## [6.2.3] - 2019-05-31
### Changed
 - Using TCP Server listen state for panel server online state

## [6.2.2] - 2019-05-16
### Changed
 - Checking for nullsigs before getting sig values

## [6.2.1] - 2019-01-10
### Changed
 - Fixed bug with hard button selection states

## [6.2.0] - 2018-10-18
### Added
 - PanelServer reports local time in ISO8601
 - Panel VoIP dialing controls support dialing booking info

## [6.1.0] - 2018-09-25
### Added
 - Added proximity features to touchscreen interfaces
 - Added console features for touchscreens
 - Added constructor for smart object collection

## [6.0.0] - 2018-09-14
### Added
 - Added Crestron panels project
 - Added panel control interfaces and abstractions
 - Added interfaces for touchscreen controls

### Changed
 - Optimizations to panel devices

## [5.2.1] - 2018-07-19
### Changed
 - ThinConferenceSource SourceType now specified as Audio

## [5.2.0] - 2018-07-02
### Changed
 - Changed Mock Panel Inputs to Public
 - MockSmartObjects now implements LastOuput
 - OnAnyOutput now works for MockSmartObjects
 - MockPanelDevice now subcribes and unsubcribes to smartObjects

## [5.1.0] - 2018-06-19
### Added
 - Added Xpanel adapter

### Changed
 - Using new conferencing interfaces for panel dialing

## [5.0.1] - 2018-05-24
### Added
 - Added heartbeat to maintain connection with Profound CUE
 
## [5.0.0] - 2018-05-09
### Added
 - Adding OnSourceRemoved event to dialers

### Changed
 - ThinConferenceSource uses delegate callbacks

## [4.0.0] - 2018-04-23
### Changed
 - Removed suffix from assembly name
 - Using new API event args
