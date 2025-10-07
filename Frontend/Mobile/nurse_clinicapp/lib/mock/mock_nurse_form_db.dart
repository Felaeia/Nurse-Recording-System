import 'package:logging/logging.dart';

class NurseForm {
  final String id;
  final String firstName;
  final String middleName;
  final String lastName;
  final String address;
  final String password;
  final String facebook;
  final String email;
  final String emergencyContact;

  NurseForm({
    required this.id,
    required this.firstName,
    required this.middleName,
    required this.lastName,
    required this.address,
    required this.password,
    required this.facebook,
    required this.email,
    required this.emergencyContact,
  });

  @override
  String toString() {
    return '(id: $id, name: $firstName $middleName $lastName, '
        'address: $address, facebook: $facebook, email: $email, '
        'emergencyContact: $emergencyContact)';
  }
}

class MockNurseFormDb {
  static final List<NurseForm> _forms = [];
  static final _logger = Logger('MockNurseFormDb');

  MockNurseFormDb() {
    Logger.root.level = Level.ALL;
    Logger.root.onRecord.listen((record) {
      // ignore: avoid_print
      print('${record.level.name}: ${record.time}: ${record.message}');
    });
  }

  void addForm(NurseForm form) {
    _forms.add(form);
    _logger.info('Wleocme! (${form.firstName} ${form.lastName})');
  }

  List<NurseForm> getAllForms() => _forms;

  void printAllForms() {
    _logger.info('Forms: ');
    for (var form in _forms) {
      _logger.info(form.toString());
    }
  }
}
