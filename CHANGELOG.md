# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [Unreleased]
### Changed
 - Using new JSON serialization/deserialization utils

## [7.0.0] - 2019-01-14
### Changed
 - Dialing features refactored to fit new conferencing interfaces

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
