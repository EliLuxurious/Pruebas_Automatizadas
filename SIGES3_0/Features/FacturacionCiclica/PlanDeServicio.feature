@ignore
@GestionPlanes
Feature: Gestión de Planes de Servicio

@GeneracionPlanActivo
Scenario: Generación inicial del Plan (Inicio -> Activo)
	Given Inicio de sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad' en 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	And Se ingresa al módulo 'Facturación Cíclica'
	And Se ingresa al submódulo 'Plan de Servicio'
	And Se selecciona 'Detalles del Plan'
	When Se configuran los límites de los comprobantes:
	| Campo          | Valor |
	| Valor mínimo   | 50    |
	| Valor máximo   | 500   |

	And Se configuran los límites de locales y usuarios:
	| Entidad  | Mínimo | Máximo |
	| Locales  | 1      | 5      |
	| Usuarios | 2      | 15     |

	And Se selecciona la pestaña 'Datos Generales'
	And Se ingresa la información básica del plan:
	| Campo           | Valor                                        |
	| Nombre del plan | Plan Agro                                    |
	| Descripción     | Plan orientadas a empresas agroindustriales. |

	And Se selecciona el ciclo de facturación 'MENSUAL'
	And Se ingresa el precio del plan '100'
	Then Se procede a 'GUARDAR' los cambios del plan
	And Se confirma el registro exitoso
