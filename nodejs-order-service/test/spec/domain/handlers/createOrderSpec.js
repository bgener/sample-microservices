const chai = require('chai'),
  expect = chai.expect,
  should = chai.should();

// TODO: how to avoid this crazy-long file path??
const handler = require('../../../../src/domain/handlers/createOrder');

describe('create order', function () {

  it('should throw when propertyCode is missing', function () {

    var message = {};
    expect(() => handler.handle(message)).to.throw();
  });
});
