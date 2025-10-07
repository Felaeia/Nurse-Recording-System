import 'dart:developer';

class PatientRecord {
  final String recordID;
  final String patientID;
  final String date;
  final String diagnosis;
  final String symptoms;
  final String treatment;
  final String notes;

  PatientRecord({
    required this.recordID,
    required this.patientID,
    required this.date,
    required this.diagnosis,
    required this.symptoms,
    required this.treatment,
    required this.notes,
  });

  @override
  String toString() {
    return 'RecordID: $recordID, PatientID: $patientID, Date: $date, Diagnosis: $diagnosis, Symptoms: $symptoms, Treatment: $treatment, Notes: $notes';
  }
}

class MockPatientRecordDb {
  MockPatientRecordDb._privateConstructor();

  static final MockPatientRecordDb _instance = MockPatientRecordDb._privateConstructor();

  factory MockPatientRecordDb() {
    return _instance;
  }

  final List<PatientRecord> _records = [];

  void addRecord(PatientRecord record) {
  _records.add(record);
  // ignore: avoid_print
  print('Record added: ${record.recordID}');
}

void printAllRecords() {
  for (var record in _records) {
    log('Record: $record');
  }
}
  List<PatientRecord> getAllRecords() => _records;

}