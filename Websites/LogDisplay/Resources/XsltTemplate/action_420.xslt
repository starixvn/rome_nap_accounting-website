<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:param name="IsInternal" select="1"></xsl:param>
  <xsl:output method="html"/>
  <xsl:template match="acc_customerupdagenthr_positiontaking">
    <table class="tblDesc">
      <xsl:for-each select="info">
        <tr class="headerDetailNormal" >
          <td colspan="6">{lbl:custInfo}</td>
        </tr>

        <tr>
          <td colspan="6">
            <div >
              <table class="tblDesc" >

                <tr >
                  <td class="colTitle">{lbl:SubAccName}</td>
                  <td class="colW2">
                    <xsl:value-of select="@subaccname" />
                  </td>
                  <td class="colTitle">{lbl:Updallmembers}</td>
                  <td class="colW2">
                    <xsl:value-of select="@updallmembers" />
                  </td>
                  <td class="colTitle">{lbl:Language}</td>
                  <td class="colW2">
                    <xsl:value-of select="@language" />
                  </td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:newmpt}</td>
                  <td class="colW2">
                    <xsl:value-of select="@newmpt" />
                  </td>
                  <td class="colTitle">{lbl:IsCopyPreset_31}</td>
                  <td class="colW2">
                    <xsl:value-of select="@iscopypreset_31" />
                  </td>
                  <td class="colTitle">{lbl:IsCopyPreset_41}</td>
                  <td class="colW2">
                    <xsl:value-of select="@iscopypreset_41" />
                  </td>                 
                </tr>
                <tr >
                  <td class="colW1">{lbl:IsCopyPreset_51}</td>
                  <td class="colW2">
                    <xsl:value-of select="@iscopypreset_51" />
                  </td>
                  <td class="colW1">{lbl:IsCopyPreset152_41}</td>
                  <td class="colW2">
                    <xsl:value-of select="@iscopypreset152_41" />                    
                  </td>
                  <td class="colW1">{lbl:IsCopyPreset152_51}</td>
                  <td class="colW2">
                    <xsl:value-of select="@iscopypreset152_51" />
                  </td>
                </tr>
                <tr >
                  <td class="colW1">{lbl:IsCopyPreset153_51}</td>
                  <td class="colW2">
                    <xsl:value-of select="@iscopypreset153_51" />
                  </td>
                  <td class="colW1">{lbl:SubAccName}</td>
                  <td class="colW2">
                    <xsl:value-of select="@bubaccname" />
                  </td>
                  <td class="colTitle">{lbl:IP}</td>
                  <td class="colW2">
                    <xsl:value-of select="@ip" />
                  </td>
                </tr>                
                <tr >                  
                  <td class="colTitle">{lbl:ErrCode}</td>
                  <td class="colW2">
                    <xsl:value-of select="@errcode" />
                  </td>

                  <td class="colTitle">{lbl:ErrMsg}</td>
                  <td class="colW2">
                    <xsl:value-of select="@errmsg" />
                  </td>
                  <td class="colTitle"></td>
                  <td class="colW2">
                  </td>
                </tr>
              </table>
            </div>
          </td>
        </tr>
      </xsl:for-each>

      <xsl:for-each select="ag">
        <tr class="headerDetailNormal" >
          <td colspan="6">{lbl:ag}</td>
        </tr>
        <tr>
          <td colspan="6">
            <div >
              <table class="tblDesc" >
                <tr>
                  <td class="colTitle">{lbl:v151apt_31}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v151apt_31" />
                  </td>
                  <td class="colTitle">{lbl:v151apt_32}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v151apt_32" />
                  </td>

                  <td class="colTitle">{lbl:v151apt_41}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v151apt_41" />
                  </td>
                </tr>
                <tr>
                  <td class="colTitle">{lbl:v151apt_42}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v151apt_42" />
                  </td>
                  <td class="colTitle">{lbl:v151apt_51}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v151apt_51" />
                  </td>
                    <td class="colTitle">{lbl:v151apt_52}</td>
                    <td class="colW2">
                      <xsl:value-of select="@v151apt_52" />
                    </td>
                </tr>
                <tr>
                  <td class="colTitle">{lbl:v152apt_41}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v152apt_41" />
                  </td>
                  <td class="colTitle">{lbl:v152apt_42}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v152apt_42" />
                  </td>
                  <td class="colTitle">{lbl:v152apt_51}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v152apt_51" />
                  </td>
                </tr>
                <tr>
                  <td class="colTitle">{lbl:v152apt_52}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v152apt_52" />
                  </td>
                  <td class="colTitle">{lbl:v153apt_51}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v153apt_51" />
                  </td>
                  <td class="colTitle">{lbl:v153apt_52}</td>
                  <td class="colW2">
                    <xsl:value-of select="@v153apt_52" />
                  </td>
                </tr>
              </table>
            </div>
          </td>
        </tr>

      </xsl:for-each>

      <xsl:for-each select="m">
        <tr class="headerDetailNormal" >
          <td colspan="6">{lbl:m}</td>
        </tr>
        <td colspan="6">
          <div >
            <table class="tblDesc" >
              <tr >
                <td class="colW1">{lbl:v151mpt_31}</td>
                <td class="colW2">
                  <xsl:value-of select="@v151mpt_31" />
                </td>
                <td class="colW1">{lbl:v151mpt_32}</td>
                <td class="colW2">
                  <xsl:value-of select="@v151mpt_32" />
                </td>

                <td class="colW1">{lbl:v151mpt_41}</td>
                <td class="colW2">
                  <xsl:value-of select="@v151mpt_41" />
                </td>
              </tr>
              <tr>
                <td class="colW1">{lbl:v151mpt_42}</td>
                <td class="colW2">
                  <xsl:value-of select="@v151mpt_42" />
                </td>
                <td class="colW1">{lbl:v151mpt_51}</td>
                <td class="colW2">
                  <xsl:value-of select="@v151mpt_51" />
                </td>
                <td class="colW1">{lbl:v151mpt_52}</td>
                <td class="colW2">
                  <xsl:value-of select="@v151mpt_52" />
                </td>
              </tr>
              <tr>
                <td class="colW1">{lbl:v152mpt_41}</td>
                <td class="colW2">
                  <xsl:value-of select="@v152mpt_41" />
                </td>
                <td class="colW1">{lbl:v152mpt_42}</td>
                <td class="colW2">
                  <xsl:value-of select="@v152mpt_42" />
                </td>
                <td class="colW1">{lbl:v152mpt_51}</td>
                <td class="colW2">
                  <xsl:value-of select="@v152mpt_51" />
                </td>
              </tr>
              <tr>
                <td class="colW1">{lbl:v152mpt_52}</td>
                <td class="colW2">
                  <xsl:value-of select="@v152mpt_52" />
                </td>
                <td class="colW1">{lbl:v153mpt_51}</td>
                <td class="colW2">
                  <xsl:value-of select="@v153mpt_51" />
                </td>
                <td class="colW1">{lbl:v153mpt_52}</td>
                <td class="colW2">
                  <xsl:value-of select="@v153mpt_52" />
                </td>
              </tr>
            </table>
          </div>
        </td>
      </xsl:for-each>

      <xsl:for-each select="aum">
        <tr class="headerDetailNormal" >
          <td colspan="6">{lbl:aum}</td>
        </tr>
        <tr>
          <td colspan="6">
            <div >
              <table class="tblDesc" >

                <tr>
                  <td class="colTitle">{lbl:a_v151mpt_31}</td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v151mpt_31" />
                  </td>
                  <td class="colTitle">{lbl:a_v151mpt_32}</td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v151mpt_32" />
                  </td>

                  <td class="colTitle">{lbl:a_v151mpt_41}</td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v151mpt_41" />
                  </td>
                </tr>
                <tr>
                  <td class="colTitle">{lbl:a_v151mpt_42}</td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v151mpt_42" />
                  </td>
                  <td class="colTitle">{lbl:a_v151mpt_51}</td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v151mpt_51" />
                  </td>
                  <td class="colTitle">{lbl:a_v151mpt_52}</td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v151mpt_52" />
                  </td>
                </tr>
                <tr>
                  <td class="colTitle">{lbl:a_v152mpt_41}</td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v152mpt_41" />
                  </td>
                  <td class="colTitle">{lbl:a_v152mpt_42}</td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v152mpt_42" />
                  </td>
                  <td class="colTitle">{lbl:a_v152mpt_51}</td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v152mpt_51" />
                  </td>
                </tr>
                <tr>
                  <td class="colTitle">{lbl:a_v152mpt_52}</td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v152mpt_52" />
                  </td>
                  <td class="colTitle">{lbl:a_v153mpt_51}</td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v153mpt_51" />
                  </td>
                  <td class="colTitle">{lbl:a_v153mpt_52}</td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v153mpt_52" />
                  </td>
                </tr>
              </table>
            </div>
          </td>
        </tr>
      </xsl:for-each>

      <xsl:for-each select="iam">
        <tr class="headerDetailNormal">
          <td colspan="6">{lbl:iam}</td>
        </tr>
        <tr>
          <td colspan="6">
            <div >
              <table class="tblDesc" >

                <tr >
                  <td class="colW1">{lbl:isa_v151mpt_31}</td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v151mpt_31" />
                  </td>
                  <td class="colW1">{lbl:isa_v151mpt_32}</td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v151mpt_32" />
                  </td>
                  <td class="colW1">{lbl:isa_v151mpt_41}</td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v151mpt_41" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">{lbl:isa_v151mpt_42}</td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v151mpt_42" />
                  </td>
                  <td class="colW1">{lbl:isa_v151mpt_51}</td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v151mpt_51" />
                  </td>
                  <td class="colW1">{lbl:isa_v151mpt_52}</td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v151mpt_52" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">{lbl:isa_v152mpt_41}</td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v152mpt_41" />
                  </td>
                  <td class="colW1">{lbl:isa_v152mpt_42}</td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v152mpt_42" />
                  </td>
                  <td class="colW1">{lbl:isa_v152mpt_51}</td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v152mpt_51" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">{lbl:isa_v152mpt_52}</td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v152mpt_52" />
                  </td>
                  <td class="colW1">{lbl:isa_v153mpt_51}</td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v153mpt_51" />
                  </td>
                  <td class="colW1">{lbl:isa_v153mpt_52}</td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v153mpt_52" />
                  </td>
                </tr>

              </table>
            </div>
          </td>
        </tr>

      </xsl:for-each>
    </table>
  </xsl:template>
</xsl:stylesheet>
