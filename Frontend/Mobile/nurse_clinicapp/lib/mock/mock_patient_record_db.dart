import '../screens/patients_record.dart';

class MockPatientRecordDb {
  final List<PatientRecord> _records = [];

  void addRecord(PatientRecord record) {
    _records.add(record);
  }

  void printAllRecords() {
    for (var record in _records) {
      print('${record.recordID} | ${record.date} | ${record.diagnosis}');
    }
  }

  List<PatientRecord> getAllRecords() => _records;
}
